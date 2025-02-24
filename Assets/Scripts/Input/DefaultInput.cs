using System;
using UnityEngine;

public class DefaultInput : MonoBehaviour, IInput
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ButtonMouseDown?.Invoke();
        else if (Input.GetMouseButton(0))
            ButtonMouseHold?.Invoke();
        else if (Input.GetMouseButtonUp(0))
            ButtonMouseUp?.Invoke();
    }

    public event Action ButtonMouseDown;
    public event Action ButtonMouseUp;
    public event Action ButtonMouseHold;

    public Vector3 MousePosition => Input.mousePosition;

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }
}