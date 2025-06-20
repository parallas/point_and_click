using Godot;
using System;
using PointAndClick.Scripts.Flipbook;

namespace PointAndClick.Scripts.Characters;
public partial class RobotChef : Node3D
{
    [Export] public FlipbookAnimator FlipbookAnimator;

    public void TestToggleAnim()
    {
        if (FlipbookAnimator.CurrentAnimation.Name == "idle")
            FaceAnimTalk();
        else
            FaceAnimIdle();
    }

    public void FaceAnimIdle()
    {
        FlipbookAnimator.StartAnimation("idle");
    }

    public void FaceAnimTalk()
    {
        FlipbookAnimator.StartAnimation("talk");
    }
}
