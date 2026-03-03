using Photon.Pun;
using UnityEngine;

public class BearAttackState : BearState
{
    private Collider _collider;
    private Animator _animator;

    public BearAttackState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        _collider = _bear.AttackCollider;
        _collider.enabled = false;
        Debug.Log("Attack 상태 돌입");
        _animator = _bear.Animator;
        _animator.SetTrigger("Attack");

    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Attack 상태 탈출");
        _animator.ResetTrigger("Attack");
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
