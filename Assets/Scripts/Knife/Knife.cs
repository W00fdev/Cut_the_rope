using System;
using DG.Tweening;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private ParticleSystem _cutEffect;
    private Vector3 _meshDefaultLocalPosition;

    private void Awake()
    {
        _meshDefaultLocalPosition = _meshRenderer.transform.localPosition;
    }

    public void Show(Vector3 position, Vector3 rightRotation, Color cutEffectColor, float cutDuration)
    {
        Transform thisTransform = transform;
        thisTransform.position = position - thisTransform.right * 0.1f;
        thisTransform.right = rightRotation;
        position += thisTransform.right * 0.15f;
        thisTransform.DOMove(position, cutDuration);

        _meshRenderer.enabled = true;
        ParticleSystem.MainModule main = _cutEffect.main;
        main.startColor = cutEffectColor;
        _cutEffect.Play();

        AnimateCut();
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
        _cutEffect.Stop();
        DOTween.Kill(_meshRenderer.transform);
        _meshRenderer.transform.localPosition = _meshDefaultLocalPosition;
    }

    private void AnimateCut()
    {
        Transform meshRendererTransform = _meshRenderer.transform;
        Vector3 animationPosition = _meshDefaultLocalPosition + Vector3.forward * 0.2f;
        meshRendererTransform.DOLocalMove(animationPosition, 0.1f).SetLoops(-1, LoopType.Yoyo);
    }
}