using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]
public class BoxSettings : ScriptableObject
{
    [SerializeField] private float _delayBeforeOpening;
    [SerializeField] private float _fillersExplosionForce;
    [SerializeField] private float _fillersExplosionRange;

    public float DelayBeforeOpening => _delayBeforeOpening;
    public float FillersExplosionForce => _fillersExplosionForce;
    public float FillersExplosionRange => _fillersExplosionRange;
}