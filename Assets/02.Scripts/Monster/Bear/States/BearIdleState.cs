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
        Debug.Log("Idle 상태 돌입");
    }

    public override void Update()
    {
        Idle();
    }

    public override void Exit()
    {
        Debug.Log("Idle 상태 탈출");
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
