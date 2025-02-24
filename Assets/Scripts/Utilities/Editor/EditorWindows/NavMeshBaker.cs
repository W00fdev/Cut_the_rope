using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;

namespace _Project.Scripts.Utilities.Editor.EditorWindows
{
    public class NavMeshBaker : EditorWindow
    {
        private IEnumerable<GameObject> _disabledObjects;
        private bool _useDisabledGameObjects;

        private void OnGUI()
        {
            _useDisabledGameObjects = GUILayout.Toggle(_useDisabledGameObjects, "Use disabled game objects");
            if (GUILayout.Button("Bake"))
                Bake();
            else if (GUILayout.Button("Clear Bake"))
                ClearBake();
        }

        [MenuItem("Tools/NavMeshBaker")]
        private static void Init()
        {
            GetWindow<NavMeshBaker>("NavMeshBaker").Show();
        }

        private void Bake()
        {
            if (_useDisabledGameObjects)
            {
                _disabledObjects = GetAllObjectsOnlyInScene().Where(go => !go.activeSelf).ToArray();
                foreach (GameObject obj in _disabledObjects)
                    obj.gameObject.SetActive(true);
            }

            NavMeshBuilder.ClearAllNavMeshes();
            NavMeshBuilder.BuildNavMesh();

            if (!_useDisabledGameObjects) return;
            foreach (GameObject obj in _disabledObjects)
                obj.gameObject.SetActive(false);
        }

        private static void ClearBake()
        {
            NavMeshBuilder.ClearAllNavMeshes();
        }

        private static IEnumerable<GameObject> GetAllObjectsOnlyInScene()
        {
            return ((GameObject[]) Resources.FindObjectsOfTypeAll(typeof(GameObject))).Where(go =>
                !EditorUtility.IsPersistent(go.transform.root.gameObject) &&
                go.hideFlags is not (HideFlags.NotEditable or HideFlags.HideAndDontSave)).ToList();
        }
    }
}