using UnityEngine;

public class BearAttackState : BearState
{
    public BearAttackState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Attack 상태 돌입");
    }

    public override void Update()
    {
        Attack();
    }

    public override void Exit()
    {
        Debug.Log("Attack 상태 탈출");
    }

    private void Attack()
    {

    }
}
