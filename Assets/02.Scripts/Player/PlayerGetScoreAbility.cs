using UnityEngine;
using System;

public class PlayerGetScoreAbility : PlayerAbility
{
    public event Action<float> OnGetScoreEvent;

    public float CurrentScore => _owner.Stat.Score;

    private void Start()
    {
        OnGetScoreEvent?.Invoke(CurrentScore);
    }

    public void AddScore(float amount)
    {
        if (!_owner.PhotonView.IsMine || amount <= 0) return;

        _owner.Stat.Score += amount;

        OnGetScoreEvent?.Invoke(CurrentScore);
    }
}
