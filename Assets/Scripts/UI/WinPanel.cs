using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : FadePanel
{
    [SerializeField] private Button _continueButton;

    private Action _onContinueButtonClickedAction;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
    }

    public void Init(Action onContinueButtonClickedAction)
    {
        _onContinueButtonClickedAction = onContinueButtonClickedAction;
    }

    private void OnContinueButtonClicked()
    {
        _onContinueButtonClickedAction?.Invoke();
    }

    protected override void OnPanelShowed()
    {
        _continueButton.transform.DOScale(Vector3.one, 0.5f);
    }

    protected override void OnPanelHided()
    {
    }
}