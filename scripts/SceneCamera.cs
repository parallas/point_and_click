using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using PointAndClick.Scripts.Interactables;

public partial class SceneCamera : Camera3D
{
    [Export] private Array<Node3D> _interactionNodes = new();
    private Array<InteractionObject> _interactionObjects = new();

    public override void _Ready()
    {
        base._Ready();

        foreach (var node in _interactionNodes)
        {
            if (node is InteractionObject baseInteractionObject)
                _interactionObjects.Add(baseInteractionObject);
            
            var children = node.FindChildren("InteractionObject");
            foreach (var child in children)
            {
                if (child is not InteractionObject interactionObject) continue;
                _interactionObjects.Add(interactionObject);
            }
        }

        if (IsCurrent()) Initialize();
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
    }
}
