using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RopesPanel : MonoBehaviour, IHidePanel
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private AbstractRopesCutter _ropesCutter;
    [SerializeField] private RopeViewElement _ropeViewElementPrefab;
    [SerializeField] private Transform _ropeViewElementParent;

    private Queue<RopeViewElement> _elements;

    private IReadOnlyList<BakedRope> Ropes => _ropesCutter.Ropes;

    public void Hide(float duration = 0f)
    {
        _canvasGroup.DOFade(0f, duration);
    }

    public void CreateElements()
    {
        _elements = new Queue<RopeViewElement>();
        for (var i = 0; i < Ropes.Count; i++)
        {
            RopeViewElement element = Instantiate(_ropeViewElementPrefab, _ropeViewElementParent);
            element.Render(Ropes[i].Color, i + 1);
            _elements.Enqueue(element);
        }
    }

    public void MarkElementAsCompleted()
    {
        _elements.Dequeue().MarkAsCompleted();
    }
}