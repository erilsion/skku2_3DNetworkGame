using Photon.Pun;
using UnityEngine;

public class BearHitState : BearState
{
    public BearHitState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Hit 상태 돌입");
        _bear.Agent.isStopped = true;
        _bear.Animator.SetTrigger("Hit");
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Hit 상태 탈출");
    }

    public void OnHitAnimationEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        _bear.Agent.isStopped = false;

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
