using TMPro;
using UnityEngine;

public class ScoreItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _rankTextUI;
    [SerializeField] private TextMeshProUGUI _nicknameTextUI;
    [SerializeField] private TextMeshProUGUI _scoreTextUI;

    public void Set(int rank, string nickname, int score)
    {
        _rankTextUI.text = $"{rank}";
        _nicknameTextUI.text = nickname;
        _scoreTextUI.text = $"{score:N0}";
    }
}
