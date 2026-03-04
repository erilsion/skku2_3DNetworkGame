using Photon.Pun;
using UnityEngine;

public class BearAttackAbility : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (other.transform == transform) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            PlayerController otherPlayer = other.GetComponent<PlayerController>();
            otherPlayer.PhotonView.RPC(nameof(damageable.TakeDamage), RpcTarget.All, otherPlayer.Stat.Damage, actorNumber);
        }
    }
}
