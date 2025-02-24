using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private BoxSettings _settings;
    [SerializeField] private BoxAnimator _animator;
    [SerializeField] private List<Filler> _fillers;
    [SerializeField] private Toy _toy;
    [SerializeField] private ParticleSystem[] _confetti;
    private Collider _collider;

    public Toy Toy => _toy;
    public BoxSettings Settings => _settings;
    public BoxAnimator Animator => _animator;

    public void Init()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _animator.TriggerExplodeFillers += ExplodeFillers;
    }

    private void OnDisable()
    {
        _animator.TriggerExplodeFillers -= ExplodeFillers;
    }

    public void Open()
    {
        _animator.Open();
    }

    public Vector3 GetClosestPoint(Vector3 position)
    {
        return _collider.ClosestPoint(position);
    }

    private void ExplodeFillers()
    {
        foreach (Filler filler in _fillers)
        {
            filler.Rigidbody.isKinematic = false;
            filler.Rigidbody.AddExplosionForce(_settings.FillersExplosionForce, transform.position,
                _settings.FillersExplosionRange);
        }

        foreach (ParticleSystem effect in _confetti)
            effect.Play();
    }

    [ContextMenu(nameof(GetFillers))]
    private void GetFillers()
    {
        _fillers = GetComponentsInChildren<Filler>().ToList();
    }
}