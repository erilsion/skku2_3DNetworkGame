using UnityEngine;

public class BearAttackWaitState : BearState
{
    private float _attackTimer = 0f;

    public BearAttackWaitState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        _attackTimer = 0f;
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
        _attackTimer += Time.deltaTime;

        _bear.Agent.SetDestination(_bear.Target.position);
        if (_bear.Target.position == null)
        {
            _attackTimer = 0f;
            _bear.ChangeState(EBearStateType.Comeback);
            return;
        }
        if (_attackTimer >= _bear.Stat.AttackCooltime)
        {
            _attackTimer = 0f;
            _bear.ChangeState(EBearStateType.Attack);
            return;
        }
        if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance > _bear.Stat.AttackRange)
        {
            _bear.ChangeState(EBearStateType.Trace);
            return;
        }
    }
}
