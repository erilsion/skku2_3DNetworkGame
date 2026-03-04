using TMPro;
using UnityEngine;

public class BearIdleState : BearState
{
    [Header("순찰 대기 관련 옵션")]
    [SerializeField] private float _idleToPatrolDelay = 4f;
    private float _idleToPatrolTimer = 0f;

    public BearIdleState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        _idleToPatrolTimer = 0f;
    }

    public override void Update()
    {
        Idle();
    }

    public override void Exit()
    {

    }

    private void Idle()
    {
        if (_bear.IsTargetInDetectRange())
        {
            _bear.ChangeState(EBearStateType.Trace);
            return;
        }

        _idleToPatrolTimer += Time.deltaTime;
        if (_idleToPatrolTimer >= _idleToPatrolDelay)
        {
            _bear.ChangeState(EBearStateType.Patrol);
            _idleToPatrolTimer = 0f;
        }
    }
}
