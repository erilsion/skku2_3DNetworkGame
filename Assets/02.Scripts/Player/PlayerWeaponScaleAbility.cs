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
        ScoreManager.OnScoreThousand += IncreaseWeaponScale;
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreThousand -= IncreaseWeaponScale;
    }

    private void IncreaseWeaponScale()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_weaponTransform == null) return;

        int level = ScoreManager.Instance.Score / _scorePerLevel;
        float scaleIncrease = level * _scaleIncreaseAmount;

        _weaponTransform.localScale =
            new Vector3((_baseScale.x + scaleIncrease), (_baseScale.y + scaleIncrease), (_baseScale.z + scaleIncrease));
    }
}
