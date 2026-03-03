using UnityEngine;

public class PlayerWeaponScaleAbility : PlayerAbility
{
    [SerializeField] private Transform _weaponTransform;

    private float _scaleIncreaseAmount = 0.1f;

    private void IncreaseWeaponScale()
    {
        if (!_owner.PhotonView.IsMine) return;
        if (_weaponTransform == null) return;

        _weaponTransform.localScale += _weaponTransform.localScale * _scaleIncreaseAmount;
    }
}
