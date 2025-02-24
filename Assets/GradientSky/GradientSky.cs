using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class GradientSky : ScriptableObject {
#if UNITY_EDITOR

    [SerializeField] private Gradient _gradient;
    [Range(2, 256)]
    [SerializeField] private int _resolution = 64;
    [SerializeField] private string _path = "Assets/T.png";
    private Color[] _colors;
    Texture2D _texture;// = new Texture2D(_resolution, _resolution);
    //private void OnValidate() {
    //    SaveTeture();
    //}


    public void SaveTeture() {

        if (_texture == null) {
            _texture = new Texture2D(_resolution, _resolution);
        }
        int size = _resolution * _resolution;
        if (_colors == null) {
            _colors = new Color[size];
        }
        if (_colors.Length != size) {
            _colors = new Color[size];
        }
        float stepSize = 1f / _resolution;
        
        for (int y = 0; y < _resolution; y++) {
            for (int x = 0; x < _resolution; x++) {
                _colors[y * _resolution + x] = _gradient.Evaluate(stepSize * y);
            }
        }
        _texture.SetPixels(0, 0, _resolution, _resolution, _colors);
        _texture.Apply();
        // Сохраняем текстуру в файл
        byte[] bytes = _texture.EncodeToPNG();
        File.WriteAllBytes(_path, bytes);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        _texture = (Texture2D)AssetDatabase.LoadAssetAtPath(_path, typeof(Texture2D));
    }

#endif
}
