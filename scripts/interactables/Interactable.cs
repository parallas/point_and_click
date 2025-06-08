using Godot;

namespace PointAndClick.Scripts.Interactables;

public partial class Interactable : StaticBody3D
{
    private bool _verboseLogging = false;
    public virtual void Hovered() { if (_verboseLogging) GD.Print($"Cursor Hovered on {Name}"); }
    public virtual void Interacted() { if (_verboseLogging) GD.Print($"Cursor Interacted on {Name}"); }
}
