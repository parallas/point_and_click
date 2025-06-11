using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ViewportRegister : Node
{
    public static readonly Dictionary<String, Viewport> Viewports = new Dictionary<String, Viewport>();
    [Export] public Viewport Viewport { get; set; }
    [Export] public String CustomName { get; set; }

    public override void _Ready()
    {
        base._Ready();

        Viewports.Add(CustomName ?? "", Viewport);
    }
}
