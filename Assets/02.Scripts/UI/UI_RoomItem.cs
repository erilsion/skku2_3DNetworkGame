using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine;
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
        if (roomInfo.CustomProperties.TryGetValue("MasterName", out object masterName))
        {
            _masterNicknameTextUI.text = $"방장: {masterName}";
        }
        _playerCountTextUI.text = $"인원: {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
    }

    public void EnterRoom()
    {
        if (_roomInfo == null) return;

        PhotonNetwork.JoinRoom(_roomInfo.Name);
    }
}
