using TMPro;
using Photon.Realtime;
using UnityEngine;

public class TotalScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        ScoreManager.OnPlayerScoreChanged += ChangeScore;
    }

    private void OnDisable()
    {
        ScoreManager.OnPlayerScoreChanged -= ChangeScore;
    }

    public void ChangeScore(Player player, int score)
    {
        SetScore(score);
    }

    private void SetScore(int totalScore)
    {
        _text.text = $"총 {totalScore:N0}점";
    }
}
