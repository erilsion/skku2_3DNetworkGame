using TMPro;
using UnityEngine;

public class TotalScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += SetScore;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= SetScore;
    }

    public void SetScore(int totalScore)
    {
        _text.text = $"총 {totalScore:N0}점";
    }
}
