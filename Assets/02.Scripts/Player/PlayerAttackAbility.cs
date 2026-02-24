using UnityEngine;

public class PlayerAttackAbility : PlayerAbility
{
    [SerializeField] private int _attackAnimationIndex = 3;
    [SerializeField] private bool _sequentialAttackAnimation = true;
    [SerializeField] private bool _randomAttackAnimation = false;

    private float _attackTimer = 0f;
    private int _attackAnimationNumber = 0;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        _attackTimer += Time.deltaTime;
        if (_attackTimer < _owner.Stat.AttackSpeed) return;

        if (!_sequentialAttackAnimation && !_randomAttackAnimation)
        {
            _randomAttackAnimation = true;
        }

        if (Input.GetMouseButtonDown(0) && _sequentialAttackAnimation)
        {
            _owner.Stat.Stamina -= _owner.Stat.AttackNeedStamina;

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
            _owner.Stat.Stamina -= _owner.Stat.AttackNeedStamina;

            _attackAnimationNumber = Random.Range(0, _attackAnimationIndex);
            _animator.SetTrigger($"Attack{_attackAnimationNumber}");
            _attackTimer = 0f;
        }
    }
}
