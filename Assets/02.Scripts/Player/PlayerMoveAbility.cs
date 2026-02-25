using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    private const float GRAVITY = 9.8f;
    private float _yVelocity = 0f;

    private CharacterController _characterController;
    private Animator _animator;

    // 1. 중력 적용하기.
    // 2. 스페이스바 누르면 점프하기.
    // 3. 플레이어 이동을 카메라가 바라보는 방향 기준으로 하기.
    // 4. Idle/Run 애니메이션을 블렌드로 적용하기.
    // 5. PlayerAttackAbility 스크립트를 만들어서,
    // - 마우스 왼쪽 클릭시마다 Attack1 / Attack2 / Attack3 애니메이션을 아래 옵션에 따라 실행시켜 주세요.
    // - 열거형 옵션 1. 순차적
    // -        옵션 2. 랜덤

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        _animator.SetFloat("Speed", direction.magnitude);
        direction.Normalize();

        direction = Camera.main.transform.TransformDirection(direction);

        _yVelocity -= GRAVITY * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded && _owner.Stat.Stamina > 0f)
        {
            _owner.Stat.Stamina -= _owner.Stat.JumpNeedStamina;
            _yVelocity = _owner.Stat.JumpPower;
        }

        direction.y = _yVelocity;

        if (Input.GetKey(KeyCode.LeftShift) && _owner.Stat.Stamina > 0f)
        {
            _owner.Stat.Stamina -= _owner.Stat.Stamina * Time.deltaTime;
            _characterController.Move(direction * Time.deltaTime * _owner.Stat.SprintSpeed);
        }
        else
        {
            if (_owner.Stat.Stamina <= _owner.Stat.MaxStamina)
            {
                _owner.Stat.Stamina += _owner.Stat.Stamina * Time.deltaTime;
            }
            _characterController.Move(direction * Time.deltaTime * _owner.Stat.MoveSpeed);
        }
    }
}
