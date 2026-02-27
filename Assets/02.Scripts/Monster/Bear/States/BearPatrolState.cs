using UnityEngine;

public class BearPatrolState : BearState
{
    [Header("순찰 포지션")]
    private Vector3[] _patrolPoints;

    [Header("순찰 지점 대기 시간")]
    private float _minPatrolWaitTime = 2f;
    private float _maxPatrolWaitTime = 6f;
    private float _patrolWaitTime;

    private int _currentPatrolIndex = 0;
    private float _patrolWaitTimer = 0f;

    public BearPatrolState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Patrol 상태 돌입");
        _patrolPoints = _bear.PatrolPositions;

        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }
        _currentPatrolIndex = SetPatrolIndex(1, _patrolPoints.Length);  // 시작 지점이 Index 0번이라 제외.
        _patrolWaitTime = SetPatrolWaitTime();
    }

    public override void Update()
    {

        Patrol();
    }

    public override void Exit()
    {
        Debug.Log("Patrol 상태 탈출");
    }

    private void Patrol()
    {
        //if (_bear.IsTargetInRange(_bear.Stat.DetectRange))
        //{
        //    _bear.ChangeState(EBearStateType.Trace);
        //    return;
        //}

        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }

        Vector3 targetPoint = _patrolPoints[_currentPatrolIndex];

        _bear.Agent.SetDestination(targetPoint);
        if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance <= _bear.Agent.stoppingDistance)
        {
            _patrolWaitTimer += Time.deltaTime;
            if (_patrolWaitTimer >= _patrolWaitTime)
            {
                _currentPatrolIndex = SetPatrolIndex(0, _patrolPoints.Length);
                _patrolWaitTime = SetPatrolWaitTime();
                _patrolWaitTimer = 0f;
            }
        }
    }

    private float SetPatrolWaitTime()
    {
        float patrolWaitTime = Random.Range(_minPatrolWaitTime, _maxPatrolWaitTime);
        return patrolWaitTime;
    }

    private int SetPatrolIndex(int min, int max)
    {
        int nextIndex;

        // 순찰 지점이 1개뿐이면 0으로 반환한다(무한루프 방지).
        if (_patrolPoints.Length <= 1) return 0;

        // 다음 지점 인덱스가 현재 인덱스와 같으면(while 조건) 인덱스를 다시 뽑는다(do).
        do
        {
            nextIndex = Random.Range(min, max);
        }
        while (nextIndex == _currentPatrolIndex);

        return nextIndex;
    }
}
