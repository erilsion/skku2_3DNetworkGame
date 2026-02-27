using UnityEngine;
using Photon.Pun;

public class PlayerNetworkIdentity : MonoBehaviourPun
{
    public int ActorNumber => photonView.Owner.ActorNumber;
}
