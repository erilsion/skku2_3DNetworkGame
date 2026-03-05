using Photon.Pun;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    private Transform target;

    [SerializeField] float offsetY = 10f;

    void LateUpdate()
    {
        if (target == null)
        {
            FindLocalPlayer();
            return;
        }

        transform.position = target.position + new Vector3(0, offsetY, 0);
        transform.rotation = Quaternion.Euler(90, target.eulerAngles.y, 0);
    }

    void FindLocalPlayer()
    {
        if (PlayerRegistry.Instance == null) return;

        int myActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        if (PlayerRegistry.Instance.Players.TryGetValue(myActorNumber, out Transform playerTransform))
        {
            target = playerTransform;
        }
    }
}
