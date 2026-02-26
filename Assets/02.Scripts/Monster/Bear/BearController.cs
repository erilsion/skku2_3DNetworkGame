using Photon.Pun;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BearController : MonoBehaviourPunCallbacks
{
    public BearStat Stat;

    public EBearStateType CurrentStateType { get; private set; }
    private IBearState _currentState;
    private Dictionary<EBearStateType, IBearState> _states;

    public NavMeshAgent Agent;
    public Transform Target;

    void Awake()
    {
        _states = new Dictionary<EBearStateType, IBearState>
        {
            //{ EBearStateType.Idle, new IdleState(this) },
            //{ EBearStateType.Patrol, new PatrolState(this) },
            //{ EBearStateType.MoveToTarget, new MoveToTargetState(this) },
            //{ EBearStateType.Return, new ReturnState(this) },
            //{ EBearStateType.Attack, new AttackState(this) },
            //{ EBearStateType.AttackWait, new AttackWaitState(this) },
            //{ EBearStateType.Hit, new HitState(this) },
            //{ EBearStateType.Dead, new DeadState(this) }
        };
    }

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Agent.enabled = false;
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            return;
        }

        ChangeState(EBearStateType.None);
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
            Agent.enabled = true;
            Agent.updatePosition = true;
            Agent.updateRotation = true;

            _currentState = null;   // 상태를 강제로 초기화한다.
            ChangeState(CurrentStateType);
        }
    }
}
