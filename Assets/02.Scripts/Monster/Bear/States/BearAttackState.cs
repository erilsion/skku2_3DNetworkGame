using Photon.Pun;
using UnityEngine;

public class BearAttackState : BearState
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Animator _animator;

    public BearAttackState(BearController bear) : base(bear)
    {

    }

    public override void Enter()
    {
        _collider.enabled = false;
        Debug.Log("Attack 상태 돌입");
        _animator.SetTrigger("Attack");

    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        Debug.Log("Attack 상태 탈출");
    }

    public void OnAttackStart()
    {
        _collider.enabled = true;
    }

    public void OnAttackFinished()
    {
        _collider.enabled = false;
    }

    public void OnAttackEnd()
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
