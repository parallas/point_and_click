using Godot;
using Parallas;

namespace PointAndClick.Scripts;
public partial class GameCursor : Control
{
    [Export] public PlayerController PlayerController;
    [Export] private TextureRect _textureRect;

    private float _cursorCurrentSpeed = 1f;

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

        Vector2 newPos = Position;
        Vector2 moveAmount = Input.GetVector(
            "cursor_left",
            "cursor_right",
            "cursor_up",
            "cursor_down"
        ).Clamp(-1f, 1f);
        if (moveAmount.LengthSquared() > 0)
            _cursorCurrentSpeed = MathUtil.ExpDecay(_cursorCurrentSpeed, 5f, 1f, (float)delta);
        else
            _cursorCurrentSpeed = 1f;
        newPos += moveAmount * 128f * _cursorCurrentSpeed * (float)delta;
        newPos = newPos.Clamp(Vector2.Zero, GetViewportRect().Size);
        SetPosition(newPos);

        if (Input.IsKeyPressed(Key.Escape)) Input.SetMouseMode(Input.MouseModeEnum.Visible);
        if (Input.IsMouseButtonPressed(MouseButton.Left)) Input.SetMouseMode(Input.MouseModeEnum.Hidden);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        InputFixer.UpdateInput(@event);

        if (@event is InputEventMouseMotion mouseEvent)
        {
            SetPosition(mouseEvent.Position, true);
        }
    }
}
