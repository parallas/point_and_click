using Godot;

namespace PointAndClick.Scripts.Interactables;

public partial class ColorChanger : Interactable
{
    [Export] private BaseMaterial3D _material;

    public override void Interacted()
    {
        _material.AlbedoColor = Color.FromHsv(GD.Randf(), 1f, 1f, 1f);
    }
}
