using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GradientSky))]
public class GradientSkyEditor : Editor {

    public override void OnInspectorGUI() {
        

        GradientSky gradientSky = target as GradientSky;
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if (EditorGUI.EndChangeCheck()) {
            gradientSky.SaveTeture();
        }

        

        //if (GUILayout.Button("SaveTexture")) {
        //    gradientSky.SaveTeture();
        //}

    }

}
