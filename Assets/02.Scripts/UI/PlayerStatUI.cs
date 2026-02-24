using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : PlayerAbility
{
    [SerializeField] private Image _healthGauge;
    [SerializeField] private Image _staminaGauge;

    private void Update()
    {
        if (_owner.PhotonView.IsMine)
        {
            _healthGauge.color = Color.greenYellow;
        }
        else
        {
            _healthGauge.color = Color.red;
        }

        if (!_owner.PhotonView.IsMine) return;

        _healthGauge.fillAmount = _owner.Stat.Health / _owner.Stat.MaxHealth;
        _staminaGauge.fillAmount = _owner.Stat.Stamina / _owner.Stat.MaxStamina;
    }
}
