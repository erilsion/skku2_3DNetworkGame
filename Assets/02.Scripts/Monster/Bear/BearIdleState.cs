using UnityEngine;

public class BearIdleState : BearState
{
    public BearIdleState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Idle 상태 돌입");
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        Debug.Log("Idle 상태 탈출");
    }
}
