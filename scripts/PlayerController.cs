using Godot;
using System;
using Parallas;
using PointAndClick.Scripts;
using PointAndClick.Scripts.Engine;
using PointAndClick.Scripts.Interactables;

namespace PointAndClick.Scripts;
public partial class PlayerController : Node3D
{
    [Export] public GameCursor GameCursor;

    public Interactable HoverTarget { get; private set; }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // raycast for interactable object
        HoverTarget = null;
        GameCursor.IsHighlighted = false;
        var viewport = GetViewport();
        var cam = viewport.GetCamera3D();
        var world = GetWorld3D();
        var didHit = PhysicsTools.CheckHit(GameCursor.Position, viewport, cam, 100, world, out var result);
        if (!didHit) return;

        // ensure object is of type interactable
        if (result.Collider is not Interactable interactable) return;
        HoverTarget = interactable;
        GameCursor.IsHighlighted = true;

        // do something
        interactable.Hover();

        if (InputFixer.ConfirmActionPressed())
        {
            interactable.Interact();
        }
    }
}
