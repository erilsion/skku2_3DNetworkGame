using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance;

    private Room _room;
    public Room Room => _room;

    public event Action OnRoomChanged;          // 방 정보가 바뀌었을 때.
    public event Action<Player> OnPlayerEnter;  // 플레이어가 들어왔을 때.
    public event Action<Player> OnPlayerLeft;   // 플레이어가 나갔을 때.

    private void Awake()
    {
        Instance = this;
    }

    // 방 입장에 성공하면 자동으로 호출되는 콜백 함수
    public override void OnJoinedRoom()
    {
        _room = PhotonNetwork.CurrentRoom;

        OnRoomChanged?.Invoke();

        // 스폰 포지션을 지정하고 플레이어를 스폰한다.
        PlayerSpawner.Instance.PlayerSpawn();
        // 리소스 폴더에서 "Player" 이름을 가진 프리팹을 생성(인스턴스화)하고, 서버에 등록도 한다.
        // ㄴ 리소스 폴더는 나쁜 것이다. 그렇기 때문에 다른 방법을 찾아보자.
    }

    // 새로운 플레이어가 방에 입장하면 자동으로 호출되는 함수이다.
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnRoomChanged?.Invoke();
        OnPlayerEnter?.Invoke(newPlayer);
    }

    // 플레이어가 방에서 퇴장하면 자동으로 호출되는 함수이다.
    public override void OnPlayerLeftRoom(Player player)
    {
        OnRoomChanged?.Invoke();
        OnPlayerLeft?.Invoke(player);
    }
}
