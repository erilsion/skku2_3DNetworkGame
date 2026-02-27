using UnityEngine;

public class BearPatrolState : BearState
{
    [Header("순찰 포지션")]
    private Transform[] _patrolPoints;

    [Header("순찰 지점 대기 시간")]
    private float _minPatrolWaitTime = 2f;
    private float _maxPatrolWaitTime = 6f;
    private float _patrolWaitTime;

    private int _currentPatrolIndex = 0;
    private float _patrolWaitTimer = 0f;
    private float _patrolArriveDistance = 0.1f;

    public BearPatrolState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        Debug.Log("Patrol 상태 돌입");
        _patrolPoints = _bear.PatrolPoints;

        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }

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
        if (_bear.IsTargetInRange(_bear.Stat.DetectRange))
        {
            _bear.ChangeState(EBearStateType.Attack);
            return;
        }

        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            _bear.ChangeState(EBearStateType.Idle);
            return;
        }

        Transform targetPoint = _patrolPoints[_currentPatrolIndex];
        _bear.Agent.SetDestination(targetPoint.position);
        float distance = Vector3.Distance(_bear.transform.position, targetPoint.position);
        if (distance <= _patrolArriveDistance)
        {
            _patrolWaitTimer += Time.deltaTime;
            if (_patrolWaitTimer >= _patrolWaitTime)
            {
                _currentPatrolIndex++;
                if (_currentPatrolIndex >= _patrolPoints.Length)
                {
                    _currentPatrolIndex = 0;
                }
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
}
