using Godot;
using System;
using Godot.Collections;

namespace PointAndClick.Scripts;

[GlobalClass]
public partial class FlipbookAnimator : Node
{
    [Signal] public delegate void OnFrameChangeEventHandler(int frame);
    [Signal] public delegate void OnUvOffsetChangeEventHandler(Vector2 uvOffset);
    [Export] public Array<FlipbookAnimation> Animations = new Array<FlipbookAnimation>();
    [Export] public Vector2I FrameSize = Vector2I.Zero;
    [Export] public Vector2I FrameWidthHeight = Vector2I.Zero;
    [Export] public Direction FrameDirection = Direction.Vertical;
    [Export] public int FrameIndex = 0;

    private FlipbookAnimation _currentAnimation = null;
    private int _pingPongDirection = 1;
    private double _frameTimer = 0f;

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

            Vector2 animationStartPosition = animationStartDirection * _currentAnimation.SheetIndex * animationDistance;
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

        if (_currentAnimation is null) return;
        if (FrameSize.X == 0 || FrameSize.Y == 0) return;
        if (FrameWidthHeight.X == 0 || FrameWidthHeight.Y == 0) return;

        var framesPerSecond = _currentAnimation.FramesPerSecond;
        var mode = _currentAnimation.Mode;
        var animationLength = _currentAnimation.AnimationLength;
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
        _currentAnimation = animation;
        FrameIndex = startingFrame;
        _pingPongDirection = 1;
        _frameTimer = _currentAnimation.FramesPerSecond;
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

        StartAnimation(newAnimation);
    }
}
