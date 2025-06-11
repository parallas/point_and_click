using Godot;

namespace PointAndClick.Scripts;
public partial class MainUI : Node
{
    [Export] public GameCursor GameCursor;
    [Export] public ScreenFader ScreenFader;

    private MainUI()
    {
        AddToGroup("MainUI");
    }
}
