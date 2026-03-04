using Photon.Realtime;
using UnityEngine;

public class PlayerWeaponScaleAbility : PlayerAbility
{
    [SerializeField] private Transform _weaponTransform;

    [SerializeField] private int _scorePerLevel = 1000;

    private float _scaleIncreaseAmount = 0.1f;
    private Vector3 _baseScale;

    private void Start()
    {
        if (_weaponTransform == null)
        {
            Debug.LogError("무기 트랜스폼이 없습니다.");
            return;
        }
        _baseScale = _weaponTransform.localScale;
        ScoreManager.OnPlayerScoreChanged += HandleScoreChanged;

        // 추후 들어올 플레이어를 위해 현재 점수 기준으로 1회 강제로 계산해서 스케일을 적용한다.
        if (_owner.PhotonView.Owner.CustomProperties.TryGetValue("score", out object scoreObject))
        {
            int score = (int)scoreObject;
            ApplyScale(score);
        }
    }

    private void OnDestroy()
    {
        ScoreManager.OnPlayerScoreChanged -= HandleScoreChanged;
    }

    private void HandleScoreChanged(Player player, int newScore)
    {
        // 이 무기의 주인과 점수 변경된 플레이어가 같을 때만 처리한다.
        if (player != _owner.PhotonView.Owner) return;

        ApplyScale(newScore);
    }

    private void ApplyScale(int score)
    {
        int level = score / _scorePerLevel;
        float scaleIncrease = level * _scaleIncreaseAmount;

        _weaponTransform.localScale = _baseScale + Vector3.one * scaleIncrease;
    }
}
