using DG.Tweening;
using UnityEngine;

public abstract class ScalePanel : MonoBehaviour, IShowPanel
{
    public void Show(float duration = 0)
    {
        transform.DOScale(Vector3.one, duration).OnComplete(OnPanelShowed);
    }

    public void Hide(float duration = 0)
    {
        transform.DOScale(Vector3.zero, duration).OnComplete(OnPanelHided);
    }

    protected abstract void OnPanelShowed();
    protected abstract void OnPanelHided();
}