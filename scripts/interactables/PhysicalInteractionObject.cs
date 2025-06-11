using Godot;
using System;

namespace PointAndClick.Scripts.Interactables;
[GlobalClass]
public partial class PhysicalInteractionObject : InteractionObject
{
    public PhysicsBody3D PhysicsBody;

    public override void _EnterTree()
    {
        base._EnterTree();
        PhysicsBody = GetParent() as PhysicsBody3D;
        if (PhysicsBody is null)
            GD.PushError($"Physics body could not be found on Physical Interaction Object: {Name}");
        PhysicsBody.SetMeta("PhysicalInteractionObject", this);
    }
}
