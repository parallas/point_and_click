using Godot;

namespace PointAndClick.Scripts.Interactables;

public partial class InteractionObject : StaticBody3D
{
    [Signal]
    public delegate void OnHoveredEventHandler();
    [Signal]
    public delegate void OnInteractedEventHandler();

    private InteractionObject()
    {
        AddToGroup("InteractionObjects");
        SetActiveState(false);
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

    public void Hover()
    {
        EmitSignalOnHovered();
    }

    public void Interact()
    {
        EmitSignalOnInteracted();
    }
}
