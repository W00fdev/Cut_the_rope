using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LosePanel : FadePanel
{
    [SerializeField] private Button _loseButton;

    private Action _onRestartButtonClickedAction;

    private void OnEnable()
    {
        _loseButton.onClick.AddListener(OnLoseButtonClicked);
    }

    private void OnDisable()
    {
        _loseButton.onClick.RemoveListener(OnLoseButtonClicked);
    }

    public void Init(Action onRestartButtonClickedAction)
    {
        _onRestartButtonClickedAction = onRestartButtonClickedAction;
    }

    protected override void OnPanelShowed()
    {
        _loseButton.transform.DOScale(Vector3.one, 0.5f);
    }

    protected override void OnPanelHided()
    {
    }

    private void OnLoseButtonClicked()
    {
        _onRestartButtonClickedAction?.Invoke();
    }
}