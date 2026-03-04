using UnityEngine;

public class BearTraceState : BearState
{
    public BearTraceState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        Trace();
    }

    public override void Exit()
    {

    }

    private void Trace()
    {
        if (_bear.Target == null)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }

        _bear.Agent.SetDestination(_bear.Target.position);
        float speedPercent = _bear.Agent.velocity.magnitude / _bear.Agent.speed;
        _bear.Animator.SetFloat("Speed", speedPercent);

        if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance <= _bear.Stat.AttackRange)
        {
            _bear.ChangeState(EBearStateType.AttackWait);
            return;
        }
        else if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance > _bear.Stat.ComebackRange)
        {
            _bear.ChangeState(EBearStateType.Comeback);
            return;
        }
    }
}
