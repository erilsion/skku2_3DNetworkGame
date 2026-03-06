using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public static PlayerSpawner Instance;

    [Header("스폰 포지션")]
    [SerializeField] private Transform[] _spawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    // 씬이 완전히 로드되고 난 후에만 플레이어를 스폰한다. (모든 클라이언트가 각자의 타이밍에 스폰 가능하다.)
    public override void OnJoinedRoom()
    {
        // 이미 게임씬에 있고, 방에 막 들어온 경우에만 스폰한다.
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            PlayerSpawn();
        }
    }

    // 씬 로드가 완료되었을 때 호출되는 Photon 콜백 메서드이다.
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 게임씬이 로드되고 네트워크에 연결되어 있으면 스폰한다.
        if (scene.name == "GameScene" && PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            PlayerSpawn();
        }
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
