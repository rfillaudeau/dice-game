using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DiceAnimation : MonoBehaviour
{
    private Dice _dice;
    private Animator _animator;

    private void Awake()
    {
        _dice = GetComponent<Dice>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _dice.onMultiplierChanged += MultiplierChanged;
        _dice.onRemoved += Removed;
    }

    private void OnDisable()
    {
        _dice.onMultiplierChanged -= MultiplierChanged;
        _dice.onRemoved -= Removed;
    }

    private void MultiplierChanged(int multiplier)
    {
        _animator.SetInteger("Multiplier", multiplier);
    }

    private void Removed()
    {
        _animator.SetTrigger("Deleted");
    }
}
