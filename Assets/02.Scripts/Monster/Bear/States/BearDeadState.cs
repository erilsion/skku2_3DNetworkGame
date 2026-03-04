using Photon.Pun;
using UnityEngine;

public class BearDeadState : BearState
{
    public BearDeadState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        _bear.Agent.isStopped = true;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        _bear.Agent.isStopped = false;
        _bear.Animator.ResetTrigger("Dead");
        PhotonNetwork.Destroy(_bear.gameObject);
    }

    public void OnDeadAnimationEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _bear.ChangeState(EBearStateType.Idle);
    }
}
