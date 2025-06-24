using System.Collections.Generic;
using System.Linq;
using Godot;
using PointAndClick.Scripts.Interactables.Components;

namespace PointAndClick.Scripts.Interactables;

[GlobalClass]
public abstract partial class InteractionObject : Node
{
    [Signal]
    public delegate void OnHoveredEventHandler();
    [Signal]
    public delegate void OnInteractedEventHandler();

    public List<InteractionComponent> InteractionComponents;

    public InteractionObject()
    {
        AddToGroup("InteractionObjects");
        SetActiveState(false);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        InteractionComponents = FindChildren("*", nameof(InteractionComponent))
            .OfType<InteractionComponent>()
            .ToList();

        GD.Print($"Interaction Components: {InteractionComponents.Count}");
    }

    public void SetActiveState(bool state)
    {
        if (state)
        {
            AddToGroup("ActiveInteractionObjects");
            RemoveFromGroup("InactiveInteractionObjects");
        }
        else
        {
            AddToGroup("InactiveInteractionObjects");
            RemoveFromGroup("ActiveInteractionObjects");
        }
    }

    public virtual bool IsHovered(Vector2 position)
    {
        return true;
    }

    public void Hover()
    {
        EmitSignalOnHovered();
        InteractionComponents.ForEach(component => component.OnHover());
    }

    public void Interact()
    {
        EmitSignalOnInteracted();
        InteractionComponents.ForEach(component => component.OnInteract());
    }
}
