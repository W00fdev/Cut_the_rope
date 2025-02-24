using System;
using _Project.Scripts.Utilities.Extensions;
using DG.Tweening;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private IInput _input;
    private Vector3 _defaultScale;

    public void Init(IInput input)
    {
        _defaultScale = transform.localScale;
        _input = input;
        _input.ButtonMouseDown += SizeDown;
        _input.ButtonMouseUp += ResetSize;
    }

    private void OnDestroy()
    {
        _input.ButtonMouseDown -= SizeDown;
        _input.ButtonMouseUp -= ResetSize;
    }

    private void Update()
    {
        if (_input != null)
            transform.position = _input.MousePosition;
    }

    private void SizeDown()
    {
        DOTween.Kill(transform);
        transform.DOScale(Vector3Extensions.SingleValue(0.7f), 0.1f);
    }

    private void ResetSize()
    {
        DOTween.Kill(transform);
        transform.DOScale(_defaultScale, 0.1f);
    }
}