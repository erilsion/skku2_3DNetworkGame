using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameTextUI;
    [SerializeField] private TextMeshProUGUI _playerCountTextUI;
    [SerializeField] private Button _roomExitButton;

    private void Start()
    {
        _roomExitButton.onClick.AddListener(ExitRoom);
        PhotonRoomManager.Instance.OnRoomChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        Room room = PhotonRoomManager.Instance.Room;
        if (room == null) return;

        // CustomRoomProperties에서 방장 닉네임을 가져온다. 없으면 "Unknown"으로 표시한다.
        string masterName = "Unknown";
        if (room.CustomProperties != null && room.CustomProperties.ContainsKey(UI_Lobby.MASTER_NAME))
        {
            masterName = room.CustomProperties[UI_Lobby.MASTER_NAME].ToString();
        }

        _roomNameTextUI.text = $"{room.Name} (Host: {masterName})";
        _playerCountTextUI.text = $"{room.PlayerCount}/{room.MaxPlayers}";
    }

    private void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
