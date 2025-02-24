using System;
using DG.Tweening;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private AbstractRopesCutter _ropesCutter;
    [SerializeField] private Box _box;
    [SerializeField] private Overlay _overlay;
    [SerializeField] private ToyFinal _toyFinal;
    private IInput _input;

    private void OnEnable()
    {
        _ropesCutter.RopeCut += OnRopeCut;
        _ropesCutter.AllRopesCut += OnAllRopesCut;
        _box.Animator.BoxOpened += OnBoxOpened;
    }

    private void OnDisable()
    {
        _ropesCutter.RopeCut -= OnRopeCut;
        _ropesCutter.AllRopesCut -= OnAllRopesCut;
        _box.Animator.BoxOpened -= OnBoxOpened;
    }

    public event Action Completed;
    public event Action Failed;

    public void Init(int levelNumber, IInput input)
    {
        _input = input;

        _ropesCutter.Init(input, _overlay);
        _overlay.Init(levelNumber, input, CompleteLevel, FailLevel);
        _box.Init();
        _toyFinal.Init(_box.Toy, _overlay);
        _input.Enable();
    }

    private void CompleteLevel()
    {
        Completed?.Invoke();
    }

    private void FailLevel()
    {
        Failed?.Invoke();
    }

    private void OnRopeCut()
    {
        _overlay.MarkRopesPanelElementAsCompleted();
    }

    private void OnAllRopesCut()
    {
        DOTween.Sequence()
            .AppendInterval(_box.Settings.DelayBeforeOpening)
            .OnComplete(_box.Open);
    }

    private void OnBoxOpened()
    {
        _input.Disable();
        DOTween.Sequence().AppendInterval(1f).OnComplete(_toyFinal.AnimateToy);
    }

    [ContextMenu(nameof(FillFields))]
    private void FillFields()
    {
        _ropesCutter = FindObjectOfType<AbstractRopesCutter>();
        _box = FindObjectOfType<Box>();
    }
}