using Godot;
using System;

namespace PointAndClick.Scripts.Interactables;
[GlobalClass]
public partial class ScreenSpaceInteractionObject : InteractionObject
{
    public enum ScreenDirection
    {
        Center,
        Top,
        Bottom,
        Left,
        Right
    }
    [Export] public ScreenDirection Direction = ScreenDirection.Center;

    public override bool IsHovered(Vector2 position)
    {
        return GetDirectionRect().HasPoint(position);
    }

    private Rect2 GetDirectionRect()
    {
        var viewportSize = GetViewport().GetVisibleRect().Size;
        var centerSize = viewportSize * 0.5f;
        var generalCenter = new Rect2(
            viewportSize.X - centerSize.X * 0.5f,
            viewportSize.Y - centerSize.Y * 0.5f,
            centerSize
        );

        var sideRectWidth = 48;
        var width = viewportSize.X;
        var height = viewportSize.Y;
        var leftRect = new Rect2(0, 0, sideRectWidth, height);
        var rightRect = new Rect2(width - sideRectWidth, 0, sideRectWidth, height);
        var topRect = new Rect2(0, 0, width, sideRectWidth);
        var bottomRect = new Rect2(0, height - sideRectWidth, width, sideRectWidth);

        return Direction switch
        {
            ScreenDirection.Center => generalCenter,
            ScreenDirection.Top => topRect,
            ScreenDirection.Bottom => bottomRect,
            ScreenDirection.Left => leftRect,
            ScreenDirection.Right => rightRect,
            _ => generalCenter
        };
    }
}
