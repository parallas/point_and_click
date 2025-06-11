using Godot;
using System;
using Parallas;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public partial class Jiggle : InteractionComponent
{
    [Export] public Node3D JiggleNode;
    [Export] public float DampingRatio = 1f;
    [Export] public float Frequency = Mathf.Pi * 2f;
    [Export] public float HitAmount = 0.5f;

    private float _jiggleAmount = 1f;
    private float _jiggleVelocity = 0f;

    public override void _Process(double delta)
    {
        base._Process(delta);

        MathUtil.Spring(
            ref _jiggleAmount,
            ref _jiggleVelocity,
            1f,
            DampingRatio,
            Frequency,
            (float)delta
        );

        JiggleNode.SetScale(MathUtil.SquashScale(_jiggleAmount));
    }

    public override void OnInteract()
    {
        JiggleHit();
    }

    public void JiggleHit()
    {
        _jiggleVelocity += HitAmount;
    }
}
