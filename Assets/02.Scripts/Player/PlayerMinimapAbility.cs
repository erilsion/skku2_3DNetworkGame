using UnityEngine;

public class PlayerMinimapAbility : PlayerAbility
{
    [Header("미니맵 카메라")]
    [SerializeField] private Camera _minimapCamera;
    [SerializeField] private float _offsetY = 10f;
    [SerializeField] private float _angleX = 90;

    private Transform _target;

    private void Start()
    {
        if (_minimapCamera == null)
        {
            _minimapCamera = GetComponent<Camera>();
        }
    }

    private void LateUpdate()
    {
        if (_target == null) return;
        _target = _owner.transform;
        if (!_owner.PhotonView.IsMine) return;

        Vector3 targetPosition = _target.position;
        Vector3 finalPosition = targetPosition + new Vector3(0f, _offsetY, 0f);

        transform.position = finalPosition;

        Vector3 targetAngle = _target.eulerAngles;
        targetAngle.x = _angleX;

        transform.eulerAngles = targetAngle;
    }
}
