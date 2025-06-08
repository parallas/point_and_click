using Godot;

namespace PointAndClick.Scripts.Interactables;

public partial class Interactable : StaticBody3D
{
    [Signal]
    public delegate void OnHoveredEventHandler();
    [Signal]
    public delegate void OnInteractedEventHandler();

    public void Hover()
    {
        EmitSignalOnHovered();
    }

    public void Interact()
    {
        EmitSignalOnInteracted();
    }
}
