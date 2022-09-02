using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ColumnSelector : MonoBehaviour
{
    private Animator _animator;

    public void SetIsSelectable(bool isSelectable)
    {
        _animator.SetBool("IsSelectable", isSelectable);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
