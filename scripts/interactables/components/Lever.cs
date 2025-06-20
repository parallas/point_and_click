using Godot;
using System;
using Parallas;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public partial class Lever : InteractionComponent
{
    [Export] private Node3D _lever;
    [Export] private float _angleRange = 23;

    [Export] public bool IsOn = true;

    public override void _Ready()
    {
        base._Ready();

        var targetAngle = new Vector3(Mathf.DegToRad(_angleRange) * (IsOn ? -1 : 1), 0f, 0f);
        _lever.Quaternion = Quaternion.FromEuler(targetAngle);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        var targetAngle = new Vector3(Mathf.DegToRad(_angleRange) * (IsOn ? -1 : 1), 0f, 0f);
        _lever.Quaternion = MathUtil.ExpDecay(
            _lever.Quaternion,
            Quaternion.FromEuler(targetAngle),
            32f,
            (float)delta
        );
    }

    public bool SetState(bool state)
    {
        if (state == IsOn) return state;
        GD.Print("Lever Toggle");
        IsOn = state;
        return IsOn;
    }
}
