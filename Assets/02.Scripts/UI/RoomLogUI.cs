using TMPro;
using UnityEngine;
using Photon.Realtime;

public class RoomLogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText;

    private void Start()
    {
        _logText.text = "방에 입장하였습니다.";
        PhotonRoomManager.Instance.OnPlayerEnter += OnPlayerEnter;
        PhotonRoomManager.Instance.OnPlayerLeft += OnPlayerLeft;

        PhotonRoomManager.Instance.OnPlayerKilled += PlayerKilledLog;
    }

    private void OnPlayerEnter(Player newPlayer)
    {
        _logText.text += "\n" + $"{newPlayer.NickName}님이 입장하였습니다.";

    }

    private void OnPlayerLeft(Player player)
    {
        _logText.text += "\n" + $"{player.NickName}님이 퇴장하였습니다.";
    }

    private void PlayerKilledLog(string attackerNickname, string victimNickname)
    {
        _logText.text += "\n" + $"{attackerNickname}님이 {victimNickname}님을 처치했습니다.";
    }
}
