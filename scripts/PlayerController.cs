using Godot;
using System;
using PointAndClick.Scripts;
using PointAndClick.Scripts.Engine;

public partial class PlayerController : Node3D
{
    public override void _Process(double delta)
    {
        base._Process(delta);

        // raycast for interactable object
        var viewport = GetViewport();
        var cam = viewport.GetCamera3D();
        var world = GetWorld3D();
        var didHit = PhysicsTools.CheckHitMouse(viewport, cam, 100, world, out var result);
        if (!didHit) return;

        // ensure object is of type interactable
        var interactable = result.Collider.As<Interactable>();
        if (interactable is null) return;

        // do something
        interactable.Hovered();
    }
}
