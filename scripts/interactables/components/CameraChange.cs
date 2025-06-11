using Godot;
using System;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public partial class CameraChange : InteractionComponent
{
    [Export] public SceneCamera TargetCamera;

    public override void OnInteract()
    {
        base.OnInteract();
        TargetCamera.Initialize();
    }
}
