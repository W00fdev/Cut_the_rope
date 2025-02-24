using TMPro;
using UnityEngine;

public class LevelNumber : MonoBehaviour
{
    public void SetValue(int levelNumber)
    {
        GetComponent<TextMeshProUGUI>().text = $"LEVEL {levelNumber}";
    }
}