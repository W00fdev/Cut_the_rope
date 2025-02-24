using UnityEngine;
using UnityEngine.UI;

public class ScissorsView : MonoBehaviour
{
    [SerializeField] private Image _image;
    private IInput _input;

    public void Awake()
    {
        _image.enabled = false;
    }

    private void OnDestroy()
    {
        UnsubscribeFromInputEvents();
    }

    private void OnValidate()
    {
        if (_image != null) return;
        var image = GetComponentInChildren<Image>();
        if (image == null)
            Debug.LogError($"Cannot assign {nameof(_image)} automatically");
        else
            _image = image;
    }

    public void Init(IInput input)
    {
        _input = input;
        SubscribeToInputEvents();
    }

    private void SubscribeToInputEvents()
    {
        _input.ButtonMouseDown += ShowImage;
        _input.ButtonMouseHold += RotateImage;
        _input.ButtonMouseUp += HideImage;
    }

    private void UnsubscribeFromInputEvents()
    {
        _input.ButtonMouseDown -= ShowImage;
        _input.ButtonMouseHold -= RotateImage;
        _input.ButtonMouseUp -= HideImage;
    }

    private void ShowImage()
    {
        transform.position = Input.mousePosition;
        _image.enabled = true;
    }

    private void RotateImage()
    {
        transform.LookAt(Input.mousePosition);
    }

    private void HideImage()
    {
        _image.enabled = false;
    }
}