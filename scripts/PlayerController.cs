using Godot;
using System;
using System.Linq;
using Godot.Collections;
using Parallas;
using PointAndClick.Scripts;
using PointAndClick.Scripts.Engine;
using PointAndClick.Scripts.Interactables;

namespace PointAndClick.Scripts;
public partial class PlayerController : Node3D
{
    [Export] public GameCursor GameCursor;

    public InteractionObject HoverTarget { get; private set; }

    public override void _Ready()
    {
        base._Ready();

        var mainUi = GetTree().GetFirstNodeInGroup("MainUI") as MainUI;
        GameCursor = mainUi!.GameCursor;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // raycast for interactable object

        HoverTarget = GetTargetInteractionObject();
        GameCursor.SetHighlighted(HoverTarget is not null);

        HoverTarget?.Hover();
        if (InputFixer.ConfirmActionPressed())
        {
            HoverTarget?.Interact();
        }
    }

    private InteractionObject GetTargetInteractionObject()
    {
        InteractionObject targetInteractionObject = null;

        // Physical Interaction Objects
        var physicalInteraction = GetPhysicalInteractionObject(GameCursor.Position);
        if (physicalInteraction is not null)
        {
            targetInteractionObject = physicalInteraction;
        }

        // Screen Space Interaction Objects
        var screenSpaceInteraction = GetScreenSpaceInteractionObject(GameCursor.Position);
        if (screenSpaceInteraction is not null)
        {
            targetInteractionObject = screenSpaceInteraction;
        }

        return targetInteractionObject;
    }

    private PhysicalInteractionObject GetPhysicalInteractionObject(Vector2 mousePosition)
    {
        // Physical Interaction Objects
        var viewport = GetViewport();
        var cam = viewport.GetCamera3D();
        var world = GetWorld3D();
        var inactiveInteractionObjects = GetTree().GetNodesInGroup("InactiveInteractionObjects")
            .OfType<PhysicalInteractionObject>()
            .Select(node => node.PhysicsBody?.GetRid())
            .OfType<Rid>()
            .ToArray();
        var didHit = PhysicsTools.CheckHit(mousePosition, viewport, cam, 100, world, out var result,
            new Array<Rid>(inactiveInteractionObjects));
        if (!didHit) return null;

        // ensure object is of type interactable
        if (!result.Collider.HasMeta("PhysicalInteractionObject")) return null;
        var physicalInteraction = result.Collider.GetMeta("PhysicalInteractionObject").As<PhysicalInteractionObject>();
        return !physicalInteraction.IsHovered(mousePosition) ? null : physicalInteraction;
    }

    private ScreenSpaceInteractionObject GetScreenSpaceInteractionObject(Vector2 mousePosition)
    {
        var screenSpaceInteractionObjects = GetTree().GetNodesInGroup("ActiveInteractionObjects")
            .OfType<ScreenSpaceInteractionObject>()
            .Where(o => o.IsHovered(mousePosition));
        return screenSpaceInteractionObjects.FirstOrDefault();
    }
}
