using Godot;
using System;

public partial class DoubleLightSwitch : Node3D
{
    [Signal] public delegate void OnRightChangeEventHandler(bool state);
    [Signal] public delegate void OnLeftChangeEventHandler(bool state);

    public void RightChange(bool isOn)
    {
        EmitSignalOnRightChange(isOn);
    }

    public void LeftChange(bool isOn)
    {
        EmitSignalOnLeftChange(isOn);
    }
}
