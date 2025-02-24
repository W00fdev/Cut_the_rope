using Obi;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RopeSaver : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Save();
    }

    [ContextMenu(nameof(Save))]
    public void Save()
    {
        var rope = GetComponent<ObiRope>();
        rope.SaveStateToBlueprint(rope.sourceBlueprint);
#if UNITY_EDITOR
        EditorUtility.SetDirty(rope.sourceBlueprint);
        AssetDatabase.SaveAssets();
#endif
    }
}