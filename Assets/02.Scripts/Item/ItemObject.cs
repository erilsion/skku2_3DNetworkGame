using Photon.Pun;
using System;
using UnityEngine;

public class ItemObject : MonoBehaviourPun
{
    [SerializeField] private float _itemScore = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponent<PlayerController>().PhotonView.IsMine) return;
            other.GetComponent<PlayerGetScoreAbility>().AddScore(_itemScore);
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
