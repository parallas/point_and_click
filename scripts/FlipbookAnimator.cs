using Godot;
using System;

[GlobalClass]
public partial class FlipbookAnimator : Node
{
    [Signal] public delegate void OnFrameChangeEventHandler(int frame);
    [Signal] public delegate void OnUvOffsetChangeEventHandler(Vector2 uvOffset);
    [Export] public Vector2I FrameSize = Vector2I.Zero;
    [Export] public Vector2I FrameWidthHeight = Vector2I.Zero;
    [Export] public Direction FrameDirection = Direction.Vertical;
    [Export(PropertyHint.Range, "0,120")] public float FramesPerSecond = 20;
    [Export] public int AnimationIndex = 0;
    [Export] public int AnimationLength = 0;
    [Export] public AnimationMode Mode = AnimationMode.Forward;
    [Export] public int FrameIndex = 0;

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

            Vector2 animationStartPosition = animationStartDirection * AnimationIndex * animationDistance;
            Vector2 framePosition = frameDirection * FrameIndex * frameDistance;

            Vector2 pixelPosition = animationStartPosition + framePosition;
            Vector2 normalizedPosition = pixelPosition / (FrameWidthHeight * FrameSize);

            return normalizedPosition;
        }
    }

    public enum Direction { Vertical, Horizontal }
    public enum AnimationMode { Forward, Backward, PingPong }

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

        if (FramesPerSecond <= 0) return;
        if (FrameSize.X == 0 || FrameSize.Y == 0) return;
        if (FrameWidthHeight.X == 0 || FrameWidthHeight.Y == 0) return;

        float frameTime = 1f / FramesPerSecond;
        _frameTimer += delta;
        while (_frameTimer >= frameTime)
        {
            _frameTimer -= frameTime;
            switch (Mode)
            {
                case AnimationMode.Forward:
                    FrameIndex = (FrameIndex + 1) % AnimationLength;
                    break;
                case AnimationMode.Backward:
                    FrameIndex = (FrameIndex - 1) % AnimationLength;
                    break;
                case AnimationMode.PingPong:
                    if (FrameIndex + _pingPongDirection >= AnimationLength || FrameIndex + _pingPongDirection < 0)
                        _pingPongDirection = -_pingPongDirection;
                    FrameIndex += _pingPongDirection;
                    break;
            }
            EmitSignalOnFrameChange(FrameIndex);
            EmitSignalOnUvOffsetChange(UvOffset);
        }
    }

    public void StartAnimation(int animationIndex, int startingFrame, int endingFrame, float framesPerSecond, AnimationMode mode)
    {
        AnimationIndex = animationIndex;
        AnimationLength = endingFrame - startingFrame;
        FrameIndex = startingFrame;
        FramesPerSecond = framesPerSecond;
        Mode = mode;
        _pingPongDirection = 1;
        _frameTimer = FramesPerSecond;
    }
}
