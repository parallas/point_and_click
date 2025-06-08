using Godot;

namespace PointAndClick.Scripts.Interactables;

public partial class ColorChanger : Node
{
    [Export] private GeometryInstance3D _geometryInstance3D;

    public override void _Ready()
    {
        base._Ready();

        var material = _geometryInstance3D.GetMaterialOverride().Duplicate() as BaseMaterial3D;
        _geometryInstance3D.SetMaterialOverride(material);
    }

    public void ChangeColor()
    {
        ((BaseMaterial3D)_geometryInstance3D.MaterialOverride).AlbedoColor = Color.FromHsv(GD.Randf(), 1f, 1f, 1f);
    }
}
