using Photon.Pun;
using System;
using UnityEngine;

public class ItemObject : MonoBehaviourPun
{
    public event Action<float> OnScoreGained;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponent<PlayerController>().PhotonView.IsMine) return;
            other.GetComponent<PlayerController>().Stat.Score += 100;
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
