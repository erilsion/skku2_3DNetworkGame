using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private GameObject MaleCharacter;
    [SerializeField] private GameObject FemaleCharacter;

    [SerializeField] private InputField NicknameInputField;
    [SerializeField] private InputField RoomnameInputField;
    [SerializeField] private Button CreateRoomButton;

    public ECharacterType _characterType;

    public void OnClickMale() => OnClickCharacterButton(ECharacterType.Male);
    public void OnClickFemale() => OnClickCharacterButton(ECharacterType.Female);

    private void OnClickCharacterButton(ECharacterType characterType)
    {
        _characterType = characterType;

        MaleCharacter.SetActive(_characterType == ECharacterType.Male);
        FemaleCharacter.SetActive(_characterType == ECharacterType.Female);
    }
}
