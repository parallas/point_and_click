using System;
using Godot;
using Godot.Collections;

namespace PointAndClick.Scripts.Engine;

public static class PhysicsTools
{
    public struct HitInfo
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Variant Collider;
        public ulong ColliderId;
        public Rid Rid;
        public int Shape;
        public Variant Metadata;

        public HitInfo()
        {

        }

        public HitInfo(Dictionary hitResult)
        {
            Position = hitResult["position"].As<Vector3>();
            Normal = hitResult["normal"].As<Vector3>();
            Collider = hitResult["collider"];
            ColliderId = hitResult["collider_id"].As<ulong>();
            Rid = hitResult["rid"].As<Rid>();
            Shape = hitResult["shape"].As<int>();
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

    public static bool CheckHitMouse(Viewport viewport, Camera3D camera3D, float distance, World3D world, out HitInfo result, Array<Rid> exclude = null)
    {
        var mousePosition = viewport.GetMousePosition();
        var from = camera3D.ProjectRayOrigin(mousePosition);
        var dir = from + camera3D.ProjectRayNormal(mousePosition) - from;
        return CheckHit(from, from + dir * distance, world, out result, exclude);
    }
}
