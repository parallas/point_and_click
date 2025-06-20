using Godot;
using System;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public partial class Toggle : InteractionComponent
{
    [Signal] public delegate void OnStateOnEventHandler();
    [Signal] public delegate void OnStateOffEventHandler();
    [Signal] public delegate void OnToggleEventHandler(bool isOn);

    [Export] public bool IsOn { get; private set; }
    [Export] private bool _runActionOnReady = false;

    public override void _Ready()
    {
        base._Ready();

        if (!_runActionOnReady) return;
        RunAction();
    }

    public override void OnInteract()
    {
        base.OnInteract();
        IsOn = !IsOn;
        EmitSignalOnToggle(IsOn);
        RunAction();
    }

    private void RunAction()
    {
        if (IsOn) EmitSignalOnStateOn();
        else EmitSignalOnStateOff();
    }
}
