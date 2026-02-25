using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance;

    [Header("스폰 포지션")]
    [SerializeField] private Transform[] _spawnPoints;

    private Room _room;
    public Room Room => _room;

    public event Action OnRoomChanged;

    private void Awake()
    {
        Instance = this;
    }

    // 방 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;

        OnRoomChanged?.Invoke();

        // 룸에 입장한 플레이어 정보
        Dictionary<int, Player> roomPlayers = PhotonNetwork.CurrentRoom.Players;
        foreach (KeyValuePair<int, Player> player in roomPlayers)
        {
            Debug.Log($"{player.Value.NickName}: {player.Value.ActorNumber}");
        }

        Transform spawnPoint = GetRandomSpawnPoint();

        // 스폰 포지션을 지정하고 플레이어를 스폰한다.
        PhotonNetwork.Instantiate("Player", spawnPoint.position, spawnPoint.rotation);
        // 리소스 폴더에서 "Player" 이름을 가진 프리팹을 생성(인스턴스화)하고, 서버에 등록도 한다.
        // ㄴ 리소스 폴더는 나쁜 것이다. 그렇기 때문에 다른 방법을 찾아보자.
    }

    public Transform GetRandomSpawnPoint()
    {
        int spawnNumber = UnityEngine.Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[spawnNumber];
    }
}
