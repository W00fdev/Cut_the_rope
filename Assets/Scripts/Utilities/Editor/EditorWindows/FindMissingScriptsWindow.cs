using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Utilities.Editor.EditorWindows
{
    public class FindMissingScriptsWindow : EditorWindow
    {
        public List<GameObject> results = new();

        private void OnGUI()
        {
            if (GUILayout.Button("Search Project"))
                SearchProject();
            if (GUILayout.Button("Search scene"))
                SearchScene();
            if (GUILayout.Button("Search Selected Objects"))
                SearchSelected();

            // src: https://answers.unity.com/questions/859554/editorwindow-display-array-dropdown.html
            var so = new SerializedObject(this);
            SerializedProperty resultsProperty = so.FindProperty(nameof(results));
            EditorGUILayout.PropertyField(resultsProperty, true);
            so.ApplyModifiedProperties();
        }

        [MenuItem("Tools/Missing Script Finder")]
        private static void Init()
        {
            GetWindow<FindMissingScriptsWindow>("Missing Script Finder").Show();
        }

        private void SearchProject()
        {
            results = AssetDatabase.FindAssets("t:Prefab")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(x => IsMissing(x, true))
                .Distinct()
                .ToList();
        }

        private void SearchScene()
        {
            results = FindObjectsOfType<GameObject>()
                .Where(x => IsMissing(x, false))
                .Distinct()
                .ToList();
        }

        private void SearchSelected()
        {
            results = Selection.gameObjects
                .Where(x => IsMissing(x, false))
                .Distinct()
                .ToList();
        }

        private static bool IsMissing(GameObject go, bool includeChildren)
        {
            Component[] components = includeChildren
                ? go.GetComponentsInChildren<Component>()
                : go.GetComponents<Component>();

            return components.Any(x => x == null);
        }
    }
}