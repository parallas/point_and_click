using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using Parallas;
using PointAndClick.Scripts;
using PointAndClick.Scripts.Interactables;

public partial class SceneCamera : Camera3D
{
    public static Action OnCameraChange;
    [Export] private Array<Node3D> _interactionNodes = new();
    [Export] public float ShiftAmount { get; private set; } = 0.1f;
    [Export] public float FlashlightAngle { get; private set; } = 20f;
    [Export] public float FlashlightBrightness { get; private set; } = 5f;
    private Array<InteractionObject> _interactionObjects = new();

    private MainUI _mainUi;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    public override void _Ready()
    {
        base._Ready();

        _initialPosition = GlobalPosition;
        _initialRotation = Quaternion;
        _mainUi = GetTree().GetFirstNodeInGroup("MainUI") as MainUI;

        foreach (var node in _interactionNodes)
        {
            if (node is InteractionObject baseInteractionObject)
                _interactionObjects.Add(baseInteractionObject);

            var children = node.FindChildren("*");
            foreach (var child in children)
            {
                if (child is not InteractionObject interactionObject) continue;
                _interactionObjects.Add(interactionObject);
            }
        }

        if (IsCurrent()) Initialize();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        var normalizedCursorPos = _mainUi.GameCursor.Position / GetViewport().GetVisibleRect().Size;
        normalizedCursorPos *= 2f;
        normalizedCursorPos -= Vector2.One;

        var shiftVector = new Vector3(normalizedCursorPos.X, -normalizedCursorPos.Y, 0f);
        shiftVector *= ShiftAmount;
        var targetPosition = _initialPosition + (_initialRotation * shiftVector);
        GlobalPosition = MathUtil.ExpDecay(GlobalPosition, targetPosition, 8f, (float)delta);
    }

    private void SetAllObjectStates(bool state)
    {
        foreach (var node in GetTree().GetNodesInGroup("InteractionObjects"))
        {
            if (node is not InteractionObject interactionObject) continue;
            interactionObject.SetActiveState(state);
        }
    }

    private void SetObjectStates(bool state)
    {
        GD.Print($"Objects: {_interactionObjects.Count}");
        foreach (var interactionObject in _interactionObjects)
        {
            interactionObject.SetActiveState(state);
        }
    }

    public void Deinitialize()
    {
        SetAllObjectStates(false);
    }

    public void Initialize()
    {
        if (GetViewport().GetCamera3D() is SceneCamera sceneCamera)
        {
            sceneCamera.Deinitialize();
            sceneCamera.SetCurrent(false);
        }
        SetObjectStates(true);
        SetCurrent(true);
        OnCameraChange?.Invoke();
    }
}
