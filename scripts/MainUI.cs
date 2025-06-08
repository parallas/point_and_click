using Godot;

namespace PointAndClick.Scripts;
public partial class MainUI : Node
{
    [Export] public GameCursor GameCursor;

    [Export]
    public PlayerController PlayerController
    {
        get => GameCursor.PlayerController;
        set => GameCursor.PlayerController = value;
    }
}
