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

    private void Start()
    {
        PlayerSpawn();
    }

    // 스폰 포지션을 지정하고 플레이어를 스폰한다.
    public void PlayerSpawn()
    {
        Transform spawnPoint = GetRandomSpawnPoint();

        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);

        // 로컬 플레이어만 UI를 연결한다.
        PhotonView photonView = player.GetComponent<PhotonView>();
        if (!photonView.IsMine) return;
    }

    public Transform GetRandomSpawnPoint()
    {
        int spawnNumber = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[spawnNumber];
    }
}
