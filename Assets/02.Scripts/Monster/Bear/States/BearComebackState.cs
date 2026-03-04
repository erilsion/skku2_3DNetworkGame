using UnityEngine;

public class BearComebackState : BearState
{
    [Header("복귀 포지션")]
    private Vector3 _comebackPoint;

    public BearComebackState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        Comeback();
    }

    public override void Exit()
    {

    }

    private void Comeback()
    {
        _comebackPoint = _bear.PatrolPositions[0];

        if (_comebackPoint == null)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }

        _bear.Agent.SetDestination(_comebackPoint);

        if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance <= _bear.Agent.stoppingDistance)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }
    }
}
