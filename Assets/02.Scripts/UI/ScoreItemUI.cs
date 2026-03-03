using TMPro;
using UnityEngine;

public class ScoreItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    [SerializeField] private TextMeshProUGUI _scoreTextUI;

    public void Set(string nickname, int score)
    {
        _nicknameTextUI.text = nickname;
        _scoreTextUI.text = $"{score:N0}";
    }
}
