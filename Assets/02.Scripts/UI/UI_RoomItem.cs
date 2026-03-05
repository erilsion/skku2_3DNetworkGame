using UnityEngine;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class UI_RoomItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomNameTextUI;
    [SerializeField] private TextMeshProUGUI _masterNicknameTextUI;
    [SerializeField] private TextMeshProUGUI _playerCountTextUI;

    [SerializeField] private Button _roomEnterButton;

    private RoomInfo _roomInfo;

    private void Start()
    {
        _roomEnterButton.onClick.AddListener(EnterRoom);
    }

    public void Init(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;

        _roomNameTextUI.text = roomInfo.Name;
        _masterNicknameTextUI.text = $"방장: {roomInfo.Name}";
        _playerCountTextUI.text = $"인원: {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
    }

    public void EnterRoom()
    {
        if (_roomInfo == null) return;
    }
}
