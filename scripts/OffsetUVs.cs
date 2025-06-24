using Godot;
using System;

[GlobalClass]
public partial class OffsetUVs : Node
{
    [Export] public MeshInstance3D TargetMesh;
    [Export] public int TargetSurfaceIndex = 0;

    [Export] public Vector2 Offset = new(0, 0);

    private Material _initialMaterial;
    private Material _material;

    public override void _EnterTree()
    {
        base._EnterTree();

        _initialMaterial ??= TargetMesh.GetActiveMaterial(0);

        _material = _initialMaterial.Duplicate() as Material;
        TargetMesh.SetSurfaceOverrideMaterial(0, _material);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        _material.Free();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (_material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("uv_offset", Offset);
        }
    }

    public void SetOffsetFlipbook(Vector2I framePosition, Vector2 textureSize)
    {
        Offset = Vector2.One / (framePosition * textureSize);
    }

    public void SetOffset(Vector2 offset)
    {
        Offset = offset;
    }
}
