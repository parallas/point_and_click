using System;
using Godot;
using Godot.Collections;

namespace PointAndClick.Scripts.Engine;

public static class PhysicsTools
{
    public readonly struct HitInfo
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly GodotObject Collider;
        public readonly ulong ColliderId;
        public readonly Rid Rid;
        public readonly int Shape;
        public readonly Variant Metadata;

        public HitInfo()
        {

        }

        public HitInfo(Dictionary hitResult)
        {
            Position = hitResult["position"].AsVector3();
            Normal = hitResult["normal"].AsVector3();
            Collider = hitResult["collider"].AsGodotObject();
            ColliderId = hitResult["collider_id"].As<ulong>();
            Rid = hitResult["rid"].AsRid();
            Shape = hitResult["shape"].AsInt32();
            hitResult.TryGetValue("metadata", out Metadata);
        }
    }
    public static bool CheckHit(Vector3 startPos, Vector3 endPos, World3D world, out HitInfo result, Array<Rid> exclude = null)
    {
        result = default;
        var spaceState = world.DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(startPos, endPos, exclude: exclude);
        var hitResult = spaceState.IntersectRay(query);
        if (hitResult is null) return false;
        if (hitResult.Count <= 0) return false;
        result = new HitInfo(hitResult);
        return true;
    }

    public static bool CheckHit(Vector2 screenPosition, Viewport viewport, Camera3D camera3D, float distance, World3D world, out HitInfo result, Array<Rid> exclude = null)
    {
        var from = camera3D.ProjectRayOrigin(screenPosition);
        var dir = from + camera3D.ProjectRayNormal(screenPosition) - from;
        return CheckHit(from, from + dir * distance, world, out result, exclude);
    }

    public static bool CheckHitMouse(Viewport viewport, Camera3D camera3D, float distance, World3D world, out HitInfo result, Array<Rid> exclude = null)
    {
        var mousePosition = viewport.GetMousePosition();
        return CheckHit(mousePosition, viewport, camera3D, distance, world, out result, exclude);
    }
}
