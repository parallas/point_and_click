using Godot;

namespace PointAndClick.Scripts;

public partial class Interactable : StaticBody3D
{
    public virtual void Hovered() { GD.Print($"Cursor Hovered on {Name}"); }
    public virtual void Clicked() { GD.Print($"Cursor Clicked on {Name}"); }
}
