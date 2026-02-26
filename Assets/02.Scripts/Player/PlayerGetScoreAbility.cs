using Photon.Pun;
using System;
using UnityEngine;

public class PlayerGetScoreAbility : PlayerAbility
{
    public PlayerStat Stat;

    public event Action<float, float> OnScoreGained;
    [SerializeField] private TotalScoreUI _totalUI;

    private void Start()
    {
        RefreshUI();
    }

    public void AddScore(float amount)
    {
        if (!_owner.PhotonView.IsMine || amount <= 0) return;

        Stat.Score += amount;

        RefreshUI();
        OnScoreGained?.Invoke(amount, Stat.Score);
    }

    private void RefreshUI()
    {
        if (_totalUI != null)
        {
            _totalUI.SetScore(Stat.Score);
        }
    }
}
