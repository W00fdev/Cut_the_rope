using DG.Tweening;
using UnityEngine;

public abstract class FadePanel : MonoBehaviour, IShowPanel
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public void Show(float duration = 0)
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.DOFade(1f, duration).OnComplete(OnPanelShowed);
    }

    public void Hide(float duration = 0)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.DOFade(0f, duration).OnComplete(OnPanelShowed);
    }

    protected abstract void OnPanelShowed();
    protected abstract void OnPanelHided();
}