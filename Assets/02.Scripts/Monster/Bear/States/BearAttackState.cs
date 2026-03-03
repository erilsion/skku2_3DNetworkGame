using Photon.Pun;
using UnityEngine;

public class BearAttackState : BearState
{
    private Collider _collider;
    private Animator _animator;

    public BearAttackState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        _collider = _bear.AttackCollider;
        _collider.enabled = false;
        Debug.Log("Attack 상태 돌입");
        _animator = _bear.Animator;
        _animator.SetTrigger("Attack");

    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Attack 상태 탈출");
    }

    public void HandleAttackStart()
    {
        _collider.enabled = true;
    }

    public void HandleAttackFinished()
    {
        _collider.enabled = false;
    }

    public void HandleAttackEnd()
    {
        _bear.ChangeState(EBearStateType.AttackWait);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (other.transform == _bear.transform) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            PlayerController otherPlayer = other.GetComponent<PlayerController>();
            otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, otherPlayer.Stat.Damage, actorNumber);
        }
    }
}
