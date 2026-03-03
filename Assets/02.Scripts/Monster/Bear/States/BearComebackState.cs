using Unity.VisualScripting;
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
        Debug.Log("Comeback 상태 돌입");
    }

    public override void Update()
    {
        Comeback();
    }

    public override void Exit()
    {
        Debug.Log("Comeback 상태 탈출");
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
        float speedPercent = _bear.Agent.velocity.magnitude / _bear.Agent.speed;
        _bear.Animator.SetFloat("Speed", speedPercent);

        if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance <= _bear.Agent.stoppingDistance)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }
    }
}
