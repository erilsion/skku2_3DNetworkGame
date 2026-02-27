using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TotalScoreUI _totalScoreUI;

    private void Awake()
    {
        Instance = this;
    }


    public void Register(PlayerGetScoreAbility ability)
    {
        ability.OnGetScoreEvent += _totalScoreUI.SetScore;
        _totalScoreUI.SetScore(ability.CurrentScore);
    }
}
