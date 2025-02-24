using System.Collections.Generic;
using Obi;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class BakedRope : MonoBehaviour
{
    private static readonly int Color1Id = Shader.PropertyToID("_Color1");
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private ObiRope _rope;
    private readonly Dictionary<int, List<Vector3>> _particlesPositions = new();
    private Collider[] _colliders;

    public Color Color { get; private set; }

    public ObiRope Rope => _rope;

    public void Init()
    {
        _colliders = GetComponentsInChildren<Collider>();
        Color = _meshRenderer.material.GetColor(Color1Id);
    }

    private void OnDestroy()
    {
        _rope.OnEndStep -= UpdateParticlesPositions;
    }

    public void EnableColliders()
    {
        foreach (Collider collider1 in _colliders)
            collider1.enabled = true;
    }

    public void DisableColliders()
    {
        foreach (Collider collider1 in _colliders)
            collider1.enabled = false;
    }

    public void ShowMesh()
    {
        _meshRenderer.enabled = true;
    }

    public void HideMesh()
    {
        _meshRenderer.enabled = false;
    }

    public void StartRecording()
    {
        for (var i = 0; i < _rope.solverIndices.Length; i++)
            _particlesPositions.Add(i, new List<Vector3>());
        _rope.OnEndStep += SaveParticlesPositions;
    }

    private void SaveParticlesPositions(ObiActor actor, float steptime)
    {
        for (var i = 0; i < _rope.solverIndices.Length; i++)
            _particlesPositions[i].Add(_rope.GetParticlePosition(_rope.solverIndices[i]));
    }

    public void Rewind()
    {
        _rope.OnEndStep -= SaveParticlesPositions;
        _rope.OnEndStep += UpdateParticlesPositions;
    }

    private void UpdateParticlesPositions(ObiActor actor, float steptime)
    {
        for (var i = 0; i < _rope.particleCount; i++)
        {
            _rope.TeleportParticle(i, _particlesPositions[i][^1]);
            if (_particlesPositions[i].Count > 1)
                _particlesPositions[i].RemoveAt(_particlesPositions[i].Count - 1);
        }
    }
}