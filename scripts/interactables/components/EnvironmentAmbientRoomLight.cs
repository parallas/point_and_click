using Godot;
using System;
using Environment = Godot.Environment;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public partial class EnvironmentAmbientRoomLight : InteractionComponent
{
    [Export] private bool _isOn = true;
    [Export] private Light3D _lightSource;
    [Export] private WorldEnvironment _worldEnvironment;

    private void SetState(bool isOn)
    {
        _isOn = isOn;
        _lightSource.Visible = _isOn;
        if (_worldEnvironment?.Environment is null) return;
        _worldEnvironment.Environment.BackgroundEnergyMultiplier = _isOn ? 1f : 0f;
    }

    public void TurnOn()
    {
        SetState(true);
    }

    public void TurnOff()
    {
        SetState(false);
    }

    public void ToggleAmbientLight()
    {
        SetState(!_isOn);
    }
}
