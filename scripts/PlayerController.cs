using Godot;
using System;
using PointAndClick.Scripts;
using PointAndClick.Scripts.Engine;
using PointAndClick.Scripts.Interactables;

namespace PointAndClick.Scripts;
public partial class PlayerController : Node3D
{
    [Export] public MainUI MainUi;

    public Interactable HoverTarget { get; private set; }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // raycast for interactable object
        HoverTarget = null;
        var viewport = GetViewport();
        var cam = viewport.GetCamera3D();
        var world = GetWorld3D();
        var didHit = PhysicsTools.CheckHit(MainUi.GameCursor.Position, viewport, cam, 100, world, out var result);
        if (!didHit) return;

        // ensure object is of type interactable
        if (result.Collider is not Interactable interactable) return;
        HoverTarget = interactable;

        // do something
        interactable.Hover();

        if (Input.IsActionJustPressed("interact"))
        {
            interactable.Interact();
        }
    }
}
