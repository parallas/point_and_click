using Godot;
using System;
using Parallas;

[GlobalClass]
public partial class ScreenFader : MarginContainer
{
    [Signal] public delegate void FadeFinishedEventHandler();

    [Export] public ColorRect ColorRect;
    [Export] public TextureRect TextureRect;
    [Export] public Viewport InGameViewport;

    private bool _isOpaque = false;
    private Tween _tween = null;

    public enum FadeTypes
    {
        Color,
        ViewportTexture
    }

    private ScreenFader()
    {
        AddToGroup("MainScreenFade");
        SetFadeValue(0f);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        RemoveFromGroup("MainScreenFade");
    }

    public void FadeToOpaque(float duration, FadeTypes fadeType = FadeTypes.Color, Action onFinish = null)
    {
        if (_isOpaque) return;

        ColorRect.SetVisible(fadeType == FadeTypes.Color);
        TextureRect.SetVisible(fadeType == FadeTypes.ViewportTexture);

        if (fadeType == FadeTypes.ViewportTexture) duration = 0f;

        var viewport = InGameViewport ?? GetViewport();
        TextureRect.Texture = ImageTexture.CreateFromImage(viewport.GetTexture().GetImage());

        _tween?.Kill();
        _tween = CreateTween();
        _tween.Finished += EmitSignalFadeFinished;
        if (onFinish is not null) _tween.Finished += onFinish;

        _tween.TweenMethod(Callable.From<float>(SetFadeValue), 0f, 1f, duration);
        _isOpaque = true;
    }

    public void FadeToClear(float duration, FadeTypes fadeType = FadeTypes.Color, Action onFinish = null)
    {
        if (!_isOpaque) return;
        _tween?.Kill();
        _tween = CreateTween();
        _tween.Finished += EmitSignalFadeFinished;
        if (onFinish is not null) _tween.Finished += onFinish;
        _tween.TweenMethod(Callable.From<float>(SetFadeValue), 1f, 0f, duration);
        _isOpaque = false;
    }

    public void SetFadeValue(float value)
    {
        Modulate = Modulate with { A = value };
    }
}
