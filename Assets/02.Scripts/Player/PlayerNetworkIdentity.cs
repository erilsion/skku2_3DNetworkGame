using UnityEngine;
using Photon.Pun;

public class PlayerNetworkIdentity : MonoBehaviourPun
{
    public int ActorNumber => photonView.Owner.ActorNumber;
    private void Start()
    {
        if (PlayerRegistry.Instance != null)
        {
            PlayerRegistry.Instance.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (PlayerRegistry.Instance != null)
        {
            PlayerRegistry.Instance.Unregister(this);
        }
    }
}
