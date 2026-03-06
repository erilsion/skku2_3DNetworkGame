using Unity.Cinemachine;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    [SerializeField] private Transform _cameraRoot;

    private float _mx;
    private float _my;

    private void Start()
    {
        if (!_owner.PhotonView.IsMine) return;
        // Cursor.lockState = CursorLockMode.Locked;
        CinemachineCamera vcam = GameObject.Find("FollowCamera").GetComponent<CinemachineCamera>();
        vcam.Follow = _cameraRoot.transform;
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mx += mouseX * _owner.Stat.RotationSpeed * Time.deltaTime;
        _my += mouseY * _owner.Stat.RotationSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, _mx, 0f);
        _cameraRoot.localRotation = Quaternion.Euler(-_my, 0f, 0f);
    }
}
