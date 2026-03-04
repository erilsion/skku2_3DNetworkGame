using Photon.Pun;
using UnityEngine;

public class BearHitState : BearState
{
    private Animator _animator;
    public BearHitState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Hit 상태 돌입");
        _bear.Agent.isStopped = true;
        _animator = _bear.Animator;
        _animator.SetTrigger("Hit");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Hit 상태 탈출");
        _bear.Agent.isStopped = false;
        _animator.ResetTrigger("Hit");
    }

    public void OnHitAnimationEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // 타겟이 있으면 다시 추적하고, 없으면 복귀 상태로 전환한다.
        if (_bear.IsTargetInDetectRange())
        {
            _bear.ChangeState(EBearStateType.Trace);
        }
        else
        {
            _bear.ChangeState(EBearStateType.Comeback);
        }
    }
}
