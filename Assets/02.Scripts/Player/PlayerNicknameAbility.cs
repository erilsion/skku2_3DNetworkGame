using TMPro;
using UnityEngine;

public class PlayerNicknameAbility : PlayerAbility
{
    [SerializeField] private TextMeshPro _nicknameTextUI;
    
    private void Start()
    {
        _nicknameTextUI.text = _owner.PhotonView.Owner.NickName;

        if (_owner.PhotonView.IsMine)
        {
            _nicknameTextUI.color = Color.cyan;
        }
        else
        {
            _nicknameTextUI.color = Color.red;
        }
    }
}
