using Photon.Pun;
using TMPro;
using UnityEngine;

public class TotalScoreUI : PlayerAbility
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetScore(float totalScore)
    {
        if (!_owner.PhotonView.IsMine || _text == null) return;
        _text.text = $"총 {totalScore:N0}점";
    }
}
