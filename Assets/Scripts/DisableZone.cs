using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DisableZone : MonoBehaviour
{
    private readonly List<Transform> _disablingObjects = new();

    private void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = other.transform;
        _disablingObjects.Add(otherTransform);
        otherTransform.DOScale(Vector3.zero, 1f)
            .OnComplete(() =>
            {
                _disablingObjects.Remove(otherTransform);
                other.gameObject.SetActive(false);
            });
    }
}