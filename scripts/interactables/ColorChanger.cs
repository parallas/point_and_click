using Godot;

namespace PointAndClick.Scripts.Interactables;

public partial class ColorChanger : Node
{
    [Export] private BaseMaterial3D _material;

    public void ChangeColor()
    {
        _material.AlbedoColor = Color.FromHsv(GD.Randf(), 1f, 1f, 1f);
    }
}
