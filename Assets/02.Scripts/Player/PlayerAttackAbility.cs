using UnityEngine;

public class PlayerAttackAbility : MonoBehaviour
{
    [SerializeField] private int _attackAnimationIndex = 3;
    [SerializeField] private float _attackCooltime = 1.2f;
    [SerializeField] private bool _sequentialAttackAnimation = true;
    [SerializeField] private bool _randomAttackAnimation = false;

    private float _attackTimer = 0f;
    private int _attackAnimationNumber = 0;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer < _attackCooltime) return;

        if (!_sequentialAttackAnimation && !_randomAttackAnimation)
        {
            _randomAttackAnimation = true;
        }

        if (Input.GetMouseButtonDown(0) && _sequentialAttackAnimation)
        {
            if (_attackAnimationNumber >= _attackAnimationIndex)
            {
                _attackAnimationNumber = 0;
            }
            _animator.SetTrigger($"Attack{_attackAnimationNumber}");
            _attackAnimationNumber += 1;
            _attackTimer = 0f;
        }

        if (Input.GetMouseButtonDown(0) && _randomAttackAnimation)
        {
            _attackAnimationNumber = Random.Range(0, _attackAnimationIndex);
            _animator.SetTrigger($"Attack{_attackAnimationNumber}");
            _attackTimer = 0f;
        }
    }
}
