using Godot;

namespace PointAndClick.Scripts;
public partial class GameCursor : Control
{
    [Export] public PlayerController PlayerController;
    [Export] private TextureRect _textureRect;

    public override void _Ready()
    {
        base._Ready();

        Input.SetMouseMode(Input.MouseModeEnum.Hidden);
        Godot.Engine.MaxFps = 120;
    }

    public override void _Process(double delta)
    {
        var atlasTexture = (AtlasTexture)_textureRect.Texture;
        atlasTexture.SetRegion(
            new Rect2(
                PlayerController.HoverTarget is not null ? 16 : 0,
                0,
                16,
                16
            )
        );
        if (Input.IsKeyPressed(Key.Escape)) Input.SetMouseMode(Input.MouseModeEnum.Visible);
        if (Input.IsMouseButtonPressed(MouseButton.Left)) Input.SetMouseMode(Input.MouseModeEnum.Hidden);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseMotion mouseEvent)
        {
            SetPosition(mouseEvent.Position, true);
        }
    }
}
