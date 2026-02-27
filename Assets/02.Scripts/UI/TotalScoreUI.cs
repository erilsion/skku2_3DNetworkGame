using TMPro;
using UnityEngine;

public class TotalScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetScore(float totalScore)
    {
        _text.text = $"총 {totalScore:N0}점";
    }
}
