using System;
using UnityEngine;

public interface IInput
{
    public Vector3 MousePosition { get; }
    public void Enable();
    public void Disable();
    public event Action ButtonMouseDown;
    public event Action ButtonMouseUp;
    public event Action ButtonMouseHold;
}