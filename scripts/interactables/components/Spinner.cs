using Godot;
using System;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public partial class Spinner : InteractionComponent
{
    [Export] public bool IsSpinning = false;
    [Export] public Node3D TargetNode;
    [Export] public float SpinSpeed = 45f;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!IsSpinning) return;

        TargetNode.RotateY(Mathf.DegToRad(SpinSpeed) * (float)delta);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        SetState(!IsSpinning);
    }

    public void SetState(bool state)
    {
        IsSpinning = state;
    }
}
