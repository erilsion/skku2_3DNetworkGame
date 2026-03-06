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

        // UI_Lobby에서 닉네임을 가져온다.
        UI_Lobby lobby = FindAnyObjectByType<UI_Lobby>();
        if (lobby != null)
        {
            string nickname = lobby.GetNickname();
            if (!string.IsNullOrEmpty(nickname))
            {
                PhotonNetwork.NickName = nickname;
            }
            else
            {
                Debug.LogWarning("닉네임을 입력해주세요!");
                return;
            }
        }

        PhotonNetwork.JoinRoom(_roomInfo.Name);
    }
}
