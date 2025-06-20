using Godot;
using System;
using System.Linq;
using Godot.Collections;

namespace PointAndClick.Scripts.Flipbook;

[GlobalClass]
public partial class FlipbookAnimator : Node
{
    [Signal] public delegate void OnFrameChangeEventHandler(int frame);
    [Signal] public delegate void OnUvOffsetChangeEventHandler(Vector2 uvOffset);
    [Export] public Array<FlipbookAnimation> Animations;
    [Export] public Vector2I FrameSize = Vector2I.Zero;
    [Export] public Vector2I FrameWidthHeight = Vector2I.Zero;
    [Export] public Direction FrameDirection = Direction.Vertical;
    [Export] public int FrameIndex = 0;

    public FlipbookAnimation CurrentAnimation { get; private set; } = null;

    private int _pingPongDirection = 1;
    private double _frameTimer = 0f;

    private System.Collections.Generic.Dictionary<String, FlipbookAnimation> _animationTable = new();

    public Vector2 UvOffset
    {
        get
        {
            Vector2 animationStartDirection = Vector2.Right;
            Vector2 frameDirection = Vector2.Down;
            int animationDistance = FrameSize.X;
            int frameDistance = FrameSize.Y;
            if (FrameDirection == Direction.Horizontal)
            {
                animationStartDirection = Vector2.Down;
                frameDirection = Vector2.Right;
                animationDistance = FrameSize.Y;
                frameDistance = FrameSize.X;
            }

            Vector2 animationStartPosition = animationStartDirection * CurrentAnimation.SheetIndex * animationDistance;
            Vector2 framePosition = frameDirection * FrameIndex * frameDistance;

            Vector2 pixelPosition = animationStartPosition + framePosition;
            Vector2 normalizedPosition = pixelPosition / (FrameWidthHeight * FrameSize);

            return normalizedPosition;
        }
    }

    public enum Direction { Vertical, Horizontal }
    public enum AnimationMode { Forward, Backward, PingPong }

    public override void _Ready()
    {
        base._Ready();

        _animationTable = Animations.ToDictionary(anim => anim.Name, anim => anim);

        if (Animations is []) return;
        StartAnimation(Animations[0]);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // if (Input.IsActionJustPressed("cancel"))
        // {
        //     if (AnimationIndex == 0)
        //         StartAnimation(1, 0, 4, 10f, AnimationMode.PingPong);
        //     else
        //         StartAnimation(0, 0, 3, 5f, AnimationMode.PingPong);
        // }

        if (CurrentAnimation is null) return;
        if (FrameSize.X == 0 || FrameSize.Y == 0) return;
        if (FrameWidthHeight.X == 0 || FrameWidthHeight.Y == 0) return;

        var framesPerSecond = CurrentAnimation.FramesPerSecond;
        var mode = CurrentAnimation.Mode;
        var animationLength = CurrentAnimation.AnimationLength;
        if (framesPerSecond <= 0) return;

        float frameTime = 1f / framesPerSecond;
        _frameTimer += delta;
        while (_frameTimer >= frameTime)
        {
            _frameTimer -= frameTime;
            switch (mode)
            {
                case AnimationMode.Forward:
                    FrameIndex = (FrameIndex + 1) % animationLength;
                    break;
                case AnimationMode.Backward:
                    FrameIndex = (FrameIndex - 1) % animationLength;
                    break;
                case AnimationMode.PingPong:
                    if (FrameIndex + _pingPongDirection >= animationLength || FrameIndex + _pingPongDirection < 0)
                        _pingPongDirection = -_pingPongDirection;
                    FrameIndex += _pingPongDirection;
                    break;
            }
            EmitSignalOnFrameChange(FrameIndex);
            EmitSignalOnUvOffsetChange(UvOffset);
        }
    }

    public void StartAnimation(FlipbookAnimation animation, int startingFrame = 0)
    {
        CurrentAnimation = animation;
        _animationTable.TryAdd(animation.Name, animation);
        FrameIndex = startingFrame;
        _pingPongDirection = 1;
        _frameTimer = 1f / animation.FramesPerSecond;
        EmitSignalOnFrameChange(FrameIndex);
        EmitSignalOnUvOffsetChange(UvOffset);
    }

    public void StartAnimation(int animationIndex, int startingFrame, int endingFrame, float framesPerSecond, AnimationMode mode)
    {
        var newAnimation = new FlipbookAnimation()
        {
            SheetIndex = animationIndex,
            AnimationLength = endingFrame - startingFrame,
            FramesPerSecond = framesPerSecond,
            Mode = mode
        };

        StartAnimation(newAnimation, startingFrame);
    }

    public void StartAnimation(String name)
    {
        if (_animationTable.TryGetValue(name, out var animation))
        {
            StartAnimation(animation);
        }
        else
        {
            GD.PrintErr($"Animation with name \"{name}\" does not exist");
        }
    }
}
