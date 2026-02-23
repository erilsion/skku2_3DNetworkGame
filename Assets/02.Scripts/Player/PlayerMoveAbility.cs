using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 2.5f;
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

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        _animator.SetFloat("Speed", direction.magnitude);
        direction.Normalize();

        direction = Camera.main.transform.TransformDirection(direction);

        _yVelocity -= GRAVITY * Time.deltaTime;
        direction.y = _yVelocity;
        if (direction.y < 0)
        {
            direction.y = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = _jumpForce;
        }

        _characterController.Move(direction * _moveSpeed * Time.deltaTime);
    }
}
