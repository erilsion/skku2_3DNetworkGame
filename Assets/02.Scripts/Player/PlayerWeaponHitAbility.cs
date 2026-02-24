using UnityEngine;

public class PlayerWeaponHitAbility : PlayerAbility
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == _owner.transform) return;

        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_owner.Stat.Damage);
        }
    }
}
