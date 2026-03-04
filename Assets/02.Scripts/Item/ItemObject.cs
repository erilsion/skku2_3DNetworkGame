using Photon.Pun;
using System;
using UnityEngine;

public class ItemObject : MonoBehaviourPun
{
    [SerializeField] private int _itemScore = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponent<PlayerController>().PhotonView.IsMine) return;
            ScoreManager.Instance.AddScore(_itemScore);
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
