using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

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

        _roomNameTextUI.text = room.Name;
        _playerCountTextUI.text = $"{room.PlayerCount}/{room.MaxPlayers}";
    }

    private void ExitRoom()
    {
        // todo.나가기 버튼
    }
}
