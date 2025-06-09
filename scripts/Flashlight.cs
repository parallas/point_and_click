using Godot;
using System;
using Parallas;
using PointAndClick.Scripts;

public partial class Flashlight : Node3D
{
    [Export] private MainUI _mainUi;

    public override void _Process(double delta)
    {
        base._Process(delta);

        var viewport = GetViewport();
        var cam = viewport.GetCamera3D();
        var from = cam.ProjectRayOrigin(_mainUi.GameCursor.Position);
        var dir = from + cam.ProjectRayNormal(_mainUi.GameCursor.Position) - from;
        var dirShift = MathUtil.ProjectOnPlane(dir, cam.GlobalBasis.Z);
        var fromShift = from + dirShift * 0.6f;
        var fromShiftHalf = from + dirShift * 0.3f;

        Position = MathUtil.ExpDecay(Position, fromShift, 16f, (float)delta);

        var targetRotation = Transform.LookingAt(fromShiftHalf + dir, Vector3.Up).Basis.GetRotationQuaternion();
        Quaternion = MathUtil.ExpDecay(Quaternion, targetRotation, 8f, (float)delta);
    }
}
