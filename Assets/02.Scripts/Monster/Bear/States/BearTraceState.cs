using UnityEngine;

public class BearTraceState : BearState
{
    public BearTraceState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Trace 상태 돌입");
    }

    public override void Update()
    {
        Trace();
    }

    public override void Exit()
    {
        Debug.Log("Trace 상태 탈출");
    }

    private void Trace()
    {
        if (_bear.Target == null)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }

        _bear.Agent.SetDestination(_bear.Target.position);

        //if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance <= _bear.Stat.AttackRange)
        //{
        //    _bear.ChangeState(EBearStateType.Attack);
        //    return;
        //}
        //else if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance > ComebackRange)
        //{
        //    _bear.ChangeState(EBearStateType.Comeback);
        //    return;
        //}
    }
}
