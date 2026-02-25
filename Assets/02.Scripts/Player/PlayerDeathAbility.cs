using UnityEngine;
using System.Collections;

public class PlayerDeathAbility : PlayerAbility
{
    private PlayerController _controller;
    private Animator _animator;
    private PlayerMoveAbility _moveAbility;
    private Coroutine _respawnCoroutine;

    [SerializeField] private float _respawnCooltime = 5.0f;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _moveAbility = _controller.GetComponent<PlayerMoveAbility>();

        _controller.OnDeathEvent += HandleDeath;
    }

    private void OnDestroy()
    {
        _controller.OnDeathEvent -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (!_owner.PhotonView.IsMine) return;

        if (_respawnCoroutine != null) return;
        _respawnCoroutine = StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        DisablePlayer();

        yield return new WaitForSeconds(_respawnCooltime);

        var spawn = PhotonServerManager.Instance.GetRandomSpawnPoint();

        _controller.transform.position = spawn.position;
        _controller.transform.rotation = spawn.rotation;
        _controller.Stat.Health = _controller.Stat.MaxHealth;
        _controller.Stat.Stamina = _controller.Stat.MaxStamina;

        EnablePlayer();
        _respawnCoroutine = null;
    }

    private void DisablePlayer()
    {
        _animator.SetTrigger("Death");
        _moveAbility.enabled = false;
    }

    private void EnablePlayer()
    {
        _animator.ResetTrigger("Death");
        _moveAbility.enabled = true;
    }
}
