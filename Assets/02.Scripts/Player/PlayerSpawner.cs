using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public static PlayerSpawner Instance;

    [Header("스폰 포지션")]
    [SerializeField] private Transform[] _spawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerSpawn()
    {
        Transform spawnPoint = GetRandomSpawnPoint();
        PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);
    }

    public Transform GetRandomSpawnPoint()
    {
        int spawnNumber = UnityEngine.Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[spawnNumber];
    }
}
