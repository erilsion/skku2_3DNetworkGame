using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class BearStatUI : MonoBehaviour
{
    [SerializeField] private Image _healthGauge;
    [SerializeField] private BearController _bear;

    private void Start()
    {
        if (_bear == null)
        {
            Debug.LogError("BearStat이 할당되지 않았습니다.");
            return;
        }
    }

    private void Update()
    {
        if (_bear != null && _bear.Stat != null)
        {
            _healthGauge.fillAmount = _bear.Stat.Health / _bear.Stat.MaxHealth;
        }
    }
}
