using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RopeViewElement : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _ropeNumber;
    [SerializeField] private Image _checkMark;

    public void Render(Color ropeColor, int ropeNumber)
    {
        _image.color = ropeColor;
        _ropeNumber.text = ropeNumber.ToString();
    }

    public void MarkAsCompleted()
    {
        _checkMark.gameObject.SetActive(true);
    }
}