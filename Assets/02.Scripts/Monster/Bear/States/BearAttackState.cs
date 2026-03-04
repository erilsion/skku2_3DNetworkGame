using Photon.Pun;
using UnityEngine;

public class BearAttackState : BearState
{
    private Collider _collider;

    public BearAttackState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        _collider = _bear.AttackCollider;
        _collider.enabled = false;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }

    public void HandleAttackStart()
    {
        _collider.enabled = true;
    }

    public void HandleAttackFinished()
    {
        _collider.enabled = false;
    }

    public void HandleAttackEnd()
    {
        _bear.ChangeState(EBearStateType.AttackWait);
    }
}
