using Godot;
using System;
using Parallas;
using PointAndClick.Scripts;

public partial class Flashlight : SpotLight3D
{
    [Export] private SpotLight3D _light;
    [Export] private float _shiftAmount = 0.6f;

    private MainUI _mainUi;
    private Camera3D _currentCamera;

    public Flashlight()
    {
        AddToGroup("Flashlight");
    }

    public override void _Ready()
    {
        base._Ready();

        _mainUi = GetTree().GetFirstNodeInGroup("MainUI") as MainUI;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        var currentCamera = GetViewport().GetCamera3D();
        if (_currentCamera != currentCamera)
        {
            _currentCamera = currentCamera;
            Reset();
            if (_currentCamera is SceneCamera sceneCamera)
                SetValues(sceneCamera);
        }
        else
        {
            GetTargetValues(out var targetPosition, out var targetRotation);
            Position = MathUtil.ExpDecay(Position, targetPosition, 16f, (float)delta);
            Quaternion = MathUtil.ExpDecay(Quaternion, targetRotation, 8f, (float)delta);
        }
    }

    private void GetTargetValues(out Vector3 targetPosition, out Quaternion targetRotation)
    {
        var from = _currentCamera.ProjectRayOrigin(_mainUi.GameCursor.Position);
        var dir = from + _currentCamera.ProjectRayNormal(_mainUi.GameCursor.Position) - from;
        var dirShift = MathUtil.ProjectOnPlane(dir, _currentCamera.GlobalBasis.Z);
        var fromShift = from + dirShift * _shiftAmount;
        var fromShiftHalf = from + dirShift * _shiftAmount * 0.5f;

        targetPosition = fromShift;

        var targetTransform = Transform with { Origin = targetPosition };
        targetRotation = targetTransform.LookingAt(fromShiftHalf + dir, Vector3.Up).Basis.GetRotationQuaternion();
    }

    public void Reset()
    {
        GD.Print("Reset Flashlight");
        GetTargetValues(out var targetPosition, out var targetRotation);
        Position = targetPosition;
        Quaternion = targetRotation;
    }

    public void SetValues(float angle, float brightness, float shiftAmount)
    {
        _light.SpotAngle = angle;
        _light.LightEnergy = brightness;
        _shiftAmount = shiftAmount;
    }

    public void SetValues(SceneCamera sceneCamera)
    {
        SetValues(sceneCamera.FlashlightAngle, sceneCamera.FlashlightBrightness, sceneCamera.ShiftAmount * 4f);
    }
}
