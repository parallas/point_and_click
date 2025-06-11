using Godot;

namespace PointAndClick.Scripts.Interactables.Components;

[GlobalClass]
public abstract partial class InteractionComponent : Node
{
    public virtual void OnHover() {}
    public virtual void OnInteract() {}
}
