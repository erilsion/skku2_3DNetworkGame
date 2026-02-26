using Photon.Pun;
using System;
using UnityEngine;

public class ItemObject : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;

    public event Action<float> OnScoreGained;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void OnTirggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Stat.Score += 100;
            ItemObjectFactory.Instance.RequestDelete(photonView.ViewID);
        }
    }
}
