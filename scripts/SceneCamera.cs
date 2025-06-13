using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using Parallas;
using PointAndClick.Scripts;
using PointAndClick.Scripts.Interactables;

public partial class SceneCamera : Camera3D
{
    public static Action<SceneCamera> OnCameraChange;
    [Export] private Array<Node> _interactionNodes = new();
    [Export] public float ShiftAmount { get; private set; } = 0.1f;
    [Export] public float FlashlightAngle { get; private set; } = 20f;
    [Export] public float FlashlightBrightness { get; private set; } = 5f;
    private Array<InteractionObject> _interactionObjects = new();

    private MainUI _mainUi;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private Viewport _viewport;

    public override void _Ready()
    {
        base._Ready();

        _initialPosition = Position;
        _initialRotation = Quaternion;
        _mainUi = GetTree().GetFirstNodeInGroup("MainUI") as MainUI;
        _viewport = GetViewport();

        _interactionNodes.Add(this);

        foreach (var node in _interactionNodes)
        {
            if (node is InteractionObject baseInteractionObject)
                _interactionObjects.Add(baseInteractionObject);

            var children = node.FindChildren("*", "InteractionObject")
                .OfType<InteractionObject>();
            foreach (var interactionObject in children)
            {
                _interactionObjects.Add(interactionObject);
            }
        }

        GD.Print($"Camera {Name} has {_interactionObjects.Count} interaction objects.");

        if (IsCurrent()) InitializeProperties();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        var normalizedCursorPos = _mainUi.GameCursor.Position / _viewport.GetVisibleRect().Size;
        normalizedCursorPos *= 2f;
        normalizedCursorPos -= Vector2.One;

        var shiftVector = new Vector3(normalizedCursorPos.X, -normalizedCursorPos.Y, 0f);
        shiftVector *= ShiftAmount;
        var targetPosition = _initialPosition + (_initialRotation * shiftVector);
        Position = MathUtil.ExpDecay(Position, targetPosition, 8f, (float)delta);

        if (IsCurrent())
        {
            _mainUi.ScreenFader.InGameViewport = _viewport;
        }
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
        var existingCam = _viewport.GetCamera3D() as SceneCamera;
        existingCam?.Deinitialize();

        var screenFader = (ScreenFader)GetTree().GetFirstNodeInGroup("MainScreenFade");
        screenFader.FadeToOpaque(0.15f, ScreenFader.FadeTypes.ViewportTexture, () =>
        {
            existingCam?.SetCurrent(false);
            InitializeProperties();
            screenFader.FadeToClear(0.15f);
        });
    }

    public void InitializeProperties()
    {
        SetObjectStates(true);
        SetCurrent(true);
        OnCameraChange?.Invoke(this);
    }
}
