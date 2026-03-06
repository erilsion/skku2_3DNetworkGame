using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviourPunCallbacks, IPunObservable
{
    public BearStat Stat;

    public EBearStateType CurrentStateType { get; private set; } = EBearStateType.None;
    private IBearState _currentState;
    private Dictionary<EBearStateType, IBearState> _states;

    public NavMeshAgent Agent => _agent;
    public Animator Animator => _animator;
    public Collider AttackCollider => _attackCollider;
    public Collider HitCollider => _hitCollider;

    [Header("곰 에이전트")]
    [SerializeField] private NavMeshAgent _agent;

    [Header("곰 애니메이터")]
    [SerializeField] private Animator _animator;

    [Header("곰 콜라이더")]
    [SerializeField] private Collider _attackCollider;
    [SerializeField] private Collider _hitCollider;

    [Header("순찰 지점 루트")]
    [SerializeField] private Transform _patrolPointRoot;

    public Vector3[] PatrolPositions { get; private set; }

    private float _bearHitInvincibleDuration = 1.06f;
    private float _bearDeadInvincibleDuration = 5f;
    private float _invincibleEndTime;

    // 데미지 처리, 상태 변경, 무적 판단을 전부 마스터 클라이언트에서만 하기 때문에, Time.time을 써도 괜찮다.
    public bool IsInvincible => Time.time < _invincibleEndTime;

    private Transform _target;
    public Transform Target => _target;

    private float _speedLerpInterpolation = 10f;
    private Vector3 _bearLastPosition;

    private void Awake()
    {
        InitPatrolPoints();

        _states = new Dictionary<EBearStateType, IBearState>
        {
            { EBearStateType.Idle, new BearIdleState(this) },
            { EBearStateType.Patrol, new BearPatrolState(this) },
            { EBearStateType.Trace, new BearTraceState(this) },
            { EBearStateType.Comeback, new BearComebackState(this) },
            { EBearStateType.Attack, new BearAttackState(this) },
            { EBearStateType.AttackWait, new BearAttackWaitState(this) },
            { EBearStateType.Hit, new BearHitState(this) },
            { EBearStateType.Dead, new BearDeadState(this) }
        };

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _attackCollider.enabled = false;
    }

    public void Start()
    {
        _bearLastPosition = transform.position;

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

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트에서 처리하는 로직이다.
            FindClosestTarget();
            _currentState?.Update();
            float speedPercent = _agent.velocity.magnitude / _agent.speed;
            _animator.SetFloat("Speed", speedPercent);
        }
        else
        {
            // 그 외 클라이언트에서 처리하는 로직이다.
            float distance = Vector3.Distance(transform.position, _bearLastPosition);
            float speed = distance / Time.deltaTime;

            float speedPercent = speed / Stat.MoveSpeed;

            float smoothSpeed = Mathf.Lerp(
                _animator.GetFloat("Speed"),
                speedPercent,
                Time.deltaTime * _speedLerpInterpolation);

            _animator.SetFloat("Speed", smoothSpeed);

            _bearLastPosition = transform.position;
        }
    }

    public void ChangeState(EBearStateType newStateType)
    {
        if (CurrentStateType == newStateType) return;

        _currentState?.Exit();
        CurrentStateType = newStateType;
        _currentState = _states[newStateType];
        _currentState?.Enter();

        // 플레이어 전체에게 애니메이션을 호출해준다.
        ApplyVisualState(newStateType);

        photonView.RPC(nameof(SyncState), RpcTarget.Others, newStateType);
    }

    [PunRPC]
    private void SyncState(EBearStateType stateType)
    {
        if (PhotonNetwork.IsMasterClient) return;

        CurrentStateType = stateType;

        ApplyVisualState(stateType);
    }

    private void ApplyVisualState(EBearStateType stateType)
    {
        _animator.ResetTrigger("Attack");
        _animator.ResetTrigger("Hit");
        _animator.ResetTrigger("Dead");

        switch (stateType)
        {
            case EBearStateType.Attack:
                _animator.SetTrigger("Attack");
                break;

            case EBearStateType.Hit:
                _animator.SetTrigger("Hit");
                break;

            case EBearStateType.Dead:
                _animator.SetTrigger("Dead");
                break;
        }
    }

    public void RequestDamage(float damage)
    {
        photonView.RPC(nameof(RPCOnTakeDamage), RpcTarget.MasterClient, damage);
    }

    [PunRPC]
    void RPCOnTakeDamage(float damage)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (IsInvincible) return;

        Stat.Health -= damage;

        if (Stat.Health <= 0)
        {
            SetInvincible(_bearDeadInvincibleDuration);
            ChangeState(EBearStateType.Dead);
        }
        else if (CurrentStateType != EBearStateType.Hit)
        {
            SetInvincible(_bearHitInvincibleDuration);
            ChangeState(EBearStateType.Hit);
        }
    }

    public void SetInvincible(float duration)
    {
        _invincibleEndTime = Time.time + duration;
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        if (_agent == null) return;

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

        float closestDistance = float.MaxValue;
        Transform detectedTarget = null;

        foreach (var player in PlayerRegistry.Instance.Players)
        {
            Transform playerTransform = player.Value;
            if (playerTransform == null) continue;

            Vector3 diff = playerTransform.position - transform.position;
            diff.y = 0f;

            float distance = diff.magnitude;

            if (distance <= Stat.DetectRange && distance < closestDistance)
            {
                closestDistance = distance;
                detectedTarget = playerTransform;
            }
        }

        if (detectedTarget != null)
        {
            _target = detectedTarget;
            return true;
        }

        return false;
    }

    public void OnAttackStart()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_currentState is BearAttackState attackState)
        {
            attackState.HandleAttackStart();
        }
    }

    public void OnAttackFinished()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_currentState is BearAttackState attackState)
        {
            attackState.HandleAttackFinished();
        }
    }

    public void OnAttackEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_currentState is BearAttackState attackState)
        {
            attackState.HandleAttackEnd();
        }
    }

    public void OnHitEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_currentState is BearHitState hitState)
        {
            hitState.OnHitAnimationEnd();
        }
    }

    public void OnDeadEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (_currentState is BearDeadState deadState)
        {
            deadState.OnDeadAnimationEnd();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 마스터 클라이언트가 데이터를 전송한다.
            stream.SendNext(Stat.Health);
            stream.SendNext((int)CurrentStateType);
        }
        else if (stream.IsReading)
        {
            // 다른 클라이언트가 데이터를 수신한다.
            Stat.Health = (float)stream.ReceiveNext();
            CurrentStateType = (EBearStateType)stream.ReceiveNext();
        }
    }
}
