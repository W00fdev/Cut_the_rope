using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BoxAnimator : MonoBehaviour
{
    private static readonly int OpenHash = Animator.StringToHash("Open");
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public event Action TriggerExplodeFillers;
    public event Action BoxOpened;

    public void Open()
    {
        _animator.SetTrigger(OpenHash);
    }

    // used by animation event
    public void InvokeExplodeFillers()
    {
        TriggerExplodeFillers?.Invoke();
    }

    // used by animation event
    public void InvokeBoxOpened()
    {
        BoxOpened?.Invoke();
    }
}