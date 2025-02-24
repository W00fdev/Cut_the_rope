using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FillerSpawner : MonoBehaviour
{
    [SerializeField] private Filler _objectPrefab;
    [SerializeField] private int _spawnCount;
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private List<MyTransform> _transforms;

    private void Awake()
    {
        _transforms.Clear();
    }

    [ContextMenu(nameof(Spawn))]
    private void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

#if UNITY_EDITOR
    [ContextMenu(nameof(SpawnFromLoad))]
    private void SpawnFromLoad()
    {
        foreach (MyTransform myTransform in _transforms)
        {
            Transform objTransform = ((Filler) PrefabUtility.InstantiatePrefab(_objectPrefab, transform)).transform;
            objTransform.SetPositionAndRotation(myTransform.Position, myTransform.Rotation);
        }
    }
#endif

    private IEnumerator SpawnRoutine()
    {
        var cooldown = new WaitForSeconds(_spawnCooldown);
        for (var i = 0; i < _spawnCount; i++)
        {
            Filler filler = Instantiate(_objectPrefab, transform.position, _objectPrefab.transform.rotation, transform);
            filler.Rigidbody.isKinematic = false;
            _transforms.Add(new MyTransform
            {
                Position = filler.transform.position,
                Rotation = filler.transform.rotation
            });
            yield return cooldown;
        }
    }

    [ContextMenu(nameof(Save))]
    private void Save()
    {
        _transforms.Clear();
        foreach (Transform child in transform)
        {
            _transforms.Add(new MyTransform
            {
                Position = child.position,
                Rotation = child.rotation
            });
        }
    }

    [ContextMenu(nameof(RemoveChildren))]
    private void RemoveChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);
    }

    [ContextMenu(nameof(Load))]
    private void Load()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.SetPositionAndRotation(_transforms[i].Position, _transforms[i].Rotation);
        }
    }

    [Serializable]
    private struct MyTransform
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }
}