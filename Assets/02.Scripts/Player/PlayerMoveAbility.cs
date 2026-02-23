using UnityEngine;

public class PlayerMoveAbility : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _jumpForce = 2.5f;
    private const float GRAVITY = 9.8f;
    private float _yVelocity = 0f;

    private CharacterController _characterController;

    // 1. 중력 적용하기.
    // 2. 스페이스바 누르면 점프하기.

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(h, 0, v);
        direction.Normalize();

        _yVelocity -= GRAVITY * Time.deltaTime;
        direction.y = _yVelocity;

        if (Input.GetKey(KeyCode.Space) && _characterController.isGrounded)
        {
            _yVelocity = _jumpForce;
        }

        _characterController.Move(direction * _moveSpeed * Time.deltaTime);
    }
}
