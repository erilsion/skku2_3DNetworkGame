using UnityEngine;

public class BearAttackWaitState : BearState
{
    public BearAttackWaitState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("AttackWait 상태 돌입");
    }

    public override void Update()
    {
        AttackWait();
    }

    public override void Exit()
    {
        Debug.Log("AttackWait 상태 탈출");
    }

    private void AttackWait()
    {

    }
}
