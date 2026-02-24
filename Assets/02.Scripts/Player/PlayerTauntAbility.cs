using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class PlayerTauntAbility : PlayerAbility
{
    private Animator _animator;
    private float _movingSpeed = 0.1f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _animator.SetTrigger("Taunt1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _animator.SetTrigger("Taunt2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _animator.SetTrigger("Taunt3");
        }
    }
}
