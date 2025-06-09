using Godot;

namespace PointAndClick.Scripts;
public partial class MainUI : Node
{
    [Export] public GameCursor GameCursor;

    private MainUI()
    {
        AddToGroup("MainUI");
    }
}
