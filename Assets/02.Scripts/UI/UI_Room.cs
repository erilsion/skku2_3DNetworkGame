using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class UI_Room : MonoBehaviourPunCallbacks
{
    private List<UI_RoomItem> _roomItems;
    private Dictionary<string, RoomInfo> _rooms = new();

    private void Awake()
    {
        _roomItems = GetComponentsInChildren<UI_RoomItem>().ToList();

        HideAllRoomUI();
    }

    private void HideAllRoomUI() 
    { 
        foreach (UI_RoomItem roomItem in _roomItems)
        {
            roomItem.gameObject.SetActive(false);
        }
    }

    // 로비에 입장 후 방 내용(개수, 이름 등)이 바뀌면 자동으로 호출되는 함수이다.
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 모든 방 UI를 비활성화한다.
        HideAllRoomUI();

        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
            {
                // 해당 방을 제거한다.
                _rooms.Remove(room.Name);
            }
            else
            {
                // 방을 추가하거나 업데이트한다.
                _rooms[room.Name] = room;
            }
        }

        int roomCount = _rooms.Count;
        List<RoomInfo> rooms = _rooms.Values.ToList();
        for (int i = 0; i < roomCount; i++)
        {
            // 방 개수만큼만 UI를 활성화한다.
            _roomItems[i].Init(rooms[i]);
            _roomItems[i].gameObject.SetActive(true);
        }
    }
}
