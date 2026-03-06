using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private GameObject MaleCharacter;
    [SerializeField] private GameObject FemaleCharacter;

    [SerializeField] private TMP_InputField NicknameInputField;
    [SerializeField] private TMP_InputField RoomnameInputField;
    [SerializeField] private Button CreateRoomButton;

    public const string MASTER_NAME = "MasterName";

    public ECharacterType _characterType;

    public void MakeRoom()
    {
        string nickname = NicknameInputField.text;
        string roomName = RoomnameInputField.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName)) return;

        PhotonNetwork.NickName = nickname;
        // 룸 옵션 정의
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;   // 룸 최대 접속자 수
        roomOptions.IsVisible = true;  // 로비에서 룸을 보여줄 것인지 (공개/비공개 여부)
        roomOptions.IsOpen = true;     // 룸의 오픈 여부

        // 커스텀 룸 프로퍼티 설정 (방장 닉네임)
        Hashtable customProps = new Hashtable();
        customProps[MASTER_NAME] = nickname;

        roomOptions.CustomRoomProperties = customProps;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { MASTER_NAME };

        // 룸 만들기
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void OnClickMale() => OnClickCharacterButton(ECharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(ECharacterType.Female);

    private void OnClickCharacterButton(ECharacterType characterType)
    {
        _characterType = characterType;

        MaleCharacter.SetActive(_characterType == ECharacterType.Male);
        FemaleCharacter.SetActive(_characterType == ECharacterType.Female);
    }

    public string GetNickname()
    {
        return NicknameInputField.text;
    }
}
