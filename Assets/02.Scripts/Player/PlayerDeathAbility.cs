using UnityEngine;
using System.Collections;


public class PlayerDeathAbility : PlayerAbility
{
    private PlayerController _controller;

    [SerializeField] private float _respawnCooltime = 5.0f;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        _controller.OnDeathEvent += HandleDeath;
    }

    private void OnDestroy()
    {
        _controller.OnDeathEvent -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (!_owner.PhotonView.IsMine) return;

        Debug.Log("사망 처리 시작");

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        DisablePlayer();

        yield return new WaitForSeconds(_respawnCooltime);

        var spawn = PhotonServerManager.Instance.GetRandomSpawnPoint();

        _controller.transform.position = spawn.position;
        _controller.transform.rotation = spawn.rotation;
        _controller.Stat.Health = _controller.Stat.MaxHealth;

        EnablePlayer();
    }

    private void DisablePlayer()
    {
        GetComponent<Collider>().enabled = false;
        // todo.이동, 입력 비활성화 등
    }

    private void EnablePlayer()
    {
        GetComponent<Collider>().enabled = true;
    }
}
