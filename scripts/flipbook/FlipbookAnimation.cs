using Godot;
using System;

namespace PointAndClick.Scripts.Flipbook;
[GlobalClass]
public partial class FlipbookAnimation : Resource
{
    [Export] public String Name;
    [Export(PropertyHint.Range, "0,120")] public float FramesPerSecond;
    [Export] public int SheetIndex = 0;
    [Export] public int AnimationLength;
    [Export] public FlipbookAnimator.AnimationMode Mode = FlipbookAnimator.AnimationMode.Forward;
}
