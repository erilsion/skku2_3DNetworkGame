using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviourPunCallbacks
{
    public BearStat Stat;

    public EBearStateType CurrentStateType { get; private set; } = EBearStateType.None;
    private IBearState _currentState;
    private Dictionary<EBearStateType, IBearState> _states;

    public NavMeshAgent Agent => _agent;

    [Header("곰 에이전트")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("순찰 지점 루트")]
    [SerializeField] private Transform _patrolPointRoot;

    public Vector3[] PatrolPositions { get; private set; }

    private Transform _target;
    public Transform Target => _target;


    void Awake()
    {
        InitPatrolPoints();

        _states = new Dictionary<EBearStateType, IBearState>
        {
            { EBearStateType.Idle, new BearIdleState(this) },
            { EBearStateType.Patrol, new BearPatrolState(this) },
            { EBearStateType.Trace, new BearTraceState(this) },
            //{ EBearStateType.Comeback, new BearComebackState(this) },
            //{ EBearStateType.Attack, new BearAttackState(this) },
            //{ EBearStateType.AttackWait, new BearAttackWaitState(this) },
            //{ EBearStateType.Hit, new BearHitState(this) },
            //{ EBearStateType.Dead, new BearDeadState(this) }
        };
    }

    public void Start()
    {
        if (PhotonRoomManager.Instance != null)
        {
            PhotonRoomManager.Instance.OnRoomJoined += SetupByMasterState;
            PhotonRoomManager.Instance.OnMasterClientChanged += SetupByMasterState;
        }
    }

    public override void OnDisable()
    {
        if (PhotonRoomManager.Instance != null)
        {
            PhotonRoomManager.Instance.OnRoomJoined -= SetupByMasterState;
            PhotonRoomManager.Instance.OnMasterClientChanged -= SetupByMasterState;
        }
    }

    private void SetupByMasterState()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _agent.enabled = true;
            _agent.updatePosition = true;
            _agent.updateRotation = true;

            _agent.speed = Stat.MoveSpeed;
            ChangeState(EBearStateType.Idle);
        }
        else
        {
            _agent.enabled = false;
            _agent.updatePosition = false;
            _agent.updateRotation = false;
        }
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _currentState?.Update();
    }

    public void ChangeState(EBearStateType newStateType)
    {
        if (CurrentStateType == newStateType) return;

        _currentState?.Exit();
        CurrentStateType = newStateType;
        _currentState = _states[newStateType];
        _currentState?.Enter();

        photonView.RPC(nameof(SyncState), RpcTarget.Others, newStateType);
    }

    [PunRPC]
    void SyncState(EBearStateType stateType)
    {
        if (PhotonNetwork.IsMasterClient) return;

        // todo. ApplyVisualState(stateType); 식으로 타 플레이어들에게 애니메이션 호출하기.
    }

    public void RequestDamage(int damage)
    {
        photonView.RPC(nameof(RPCOnTakeDamage), RpcTarget.MasterClient, damage);
    }

    [PunRPC]
    void RPCOnTakeDamage(int damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Stat.Health -= damage;

        if (Stat.Health <= 0)
        {
            ChangeState(EBearStateType.Dead);
        }
        else
        {
            ChangeState(EBearStateType.Hit);
        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _agent.enabled = true;
            _agent.updatePosition = true;
            _agent.updateRotation = true;

            _currentState = null;   // 상태를 강제로 초기화한다.
            ChangeState(CurrentStateType);
        }
    }

    private void InitPatrolPoints()
    {
        if (_patrolPointRoot == null)
        {
            Debug.LogWarning("순찰 지점 루트가 연결되지 않았습니다.");
            PatrolPositions = new Vector3[0];
            return;
        }

        int childCount = _patrolPointRoot.childCount;
        PatrolPositions = new Vector3[childCount];

        for (int i = 0; i < childCount; i++)
        {
            PatrolPositions[i] = _patrolPointRoot.GetChild(i).position;
        }
    }

    public void FindClosestTarget()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _target = PlayerRegistry.Instance.GetClosestPlayer(transform.position);
    }

    public bool IsTargetInDetectRange()
    {
        if (!PhotonNetwork.IsMasterClient) return false;

        FindClosestTarget();

        if (_target == null) return false;

        Vector3 bearPosition = transform.position;
        Vector3 targetPosition = _target.position;

        float distance = Vector3.Distance(new Vector3(bearPosition.x, 0, bearPosition.z),
                         new Vector3(targetPosition.x, 0, targetPosition.z));

        return distance <= Stat.DetectRange;
    }
}
