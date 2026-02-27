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

        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);

        // 로컬 플레이어만 UI를 연결한다.
        PhotonView photonView = player.GetComponent<PhotonView>();
        if (!photonView.IsMine) return;

        PlayerGetScoreAbility ability = player.GetComponent<PlayerGetScoreAbility>();

        ScoreManager.Instance.Register(ability);
    }

    public Transform GetRandomSpawnPoint()
    {
        int spawnNumber = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[spawnNumber];
    }
}
