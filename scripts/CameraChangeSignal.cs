using Godot;

namespace PointAndClick.Scripts;

[GlobalClass]
public partial class CameraChangeSignal : Node
{
    [Signal] private delegate void OnCameraChangeEventHandler(SceneCamera newSceneCamera);

    public override void _EnterTree()
    {
        base._EnterTree();
        SceneCamera.OnCameraChange += EmitSignalOnCameraChange;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        SceneCamera.OnCameraChange -= EmitSignalOnCameraChange;
    }
}
