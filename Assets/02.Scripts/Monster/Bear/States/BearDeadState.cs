using Photon.Pun;
using UnityEngine;

public class BearDeadState : BearState
{
    private Animator _animator;

    public BearDeadState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Dead 상태 돌입");
        _bear.Agent.isStopped = true;
        _animator = _bear.Animator;
        _animator.SetTrigger("Dead");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Dead 상태 탈출");
        _bear.Agent.isStopped = false;
        _animator.ResetTrigger("Dead");
        PhotonNetwork.Destroy(_bear.gameObject);
    }

    public void OnDeadAnimationEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _bear.ChangeState(EBearStateType.Idle);
    }
}
