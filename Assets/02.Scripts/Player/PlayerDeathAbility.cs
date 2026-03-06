using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerDeathAbility : PlayerAbility
{
    private PlayerController _controller;
    private CharacterController _characterController;
    private Animator _animator;
    private PlayerMoveAbility _moveAbility;

    private void Start()
    {
        _controller = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        _moveAbility = _controller.GetComponent<PlayerMoveAbility>();
        _characterController = GetComponent<CharacterController>();

        _controller.OnDeathEvent += HandleDeath;
    }

    private void OnDestroy()
    {
        _controller.OnDeathEvent -= HandleDeath;
    }

    private void HandleDeath()
    {
        if (!_owner.PhotonView.IsMine) return;

        ScoreManager.Instance.HalfScore();
        _owner.PhotonView.RPC(nameof(DisablePlayer), RpcTarget.All);
    }

    public void RespawnPlayer()
    {
        if (_owner.PhotonView.IsMine)
        {
            _controller.Stat.Health = _controller.Stat.MaxHealth;
            _controller.Stat.Stamina = _controller.Stat.MaxStamina;

            Transform spawn = PlayerSpawner.Instance.GetRandomSpawnPoint();

            _characterController.enabled = false;
            _owner.transform.position = spawn.position;
            _owner.transform.rotation = spawn.rotation;
            _characterController.enabled = true;
            PhotonNetwork.SendAllOutgoingCommands();
        }
        _owner.PhotonView.RPC(nameof(EnablePlayer), RpcTarget.All);
    }

    [PunRPC]
    private void DisablePlayer()
    {
        _animator.SetTrigger("Death");
        _moveAbility.enabled = false;
    }

    [PunRPC]
    private void EnablePlayer()
    {
        _animator.ResetTrigger("Death");
        _moveAbility.enabled = true;
    }
}
