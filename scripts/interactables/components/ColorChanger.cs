using Godot;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public partial class ColorChanger : InteractionComponent
{
    [Export] private GeometryInstance3D _geometryInstance3D;

    public override void OnInteract()
    {
        base.OnInteract();
        ChangeColor();
    }

    public void ChangeColor()
    {
        var randomColor = Color.FromHsv(GD.Randf(), 1f, 1f, 1f);
        _geometryInstance3D.SetInstanceShaderParameter("tint", randomColor);
    }
}
