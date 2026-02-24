using UnityEngine;

public class PlayerTauntAbility : PlayerAbility
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            _animator.Play("Taunt1");
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            _animator.Play("Taunt2");
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            _animator.Play("Taunt3");
        }
    }
}
