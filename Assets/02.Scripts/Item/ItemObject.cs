using Photon.Pun;
using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public event Action<float> OnScoreGained;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Stat.Score += 100;
            OnScoreGained?.Invoke(gameObject.GetComponent<PlayerController>().Stat.Score);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
