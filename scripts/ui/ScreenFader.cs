using Godot;
using System;
using Parallas;

public partial class ScreenFader : ColorRect
{
    [Signal] public delegate void FadeFinishedEventHandler();

    private bool _isOpaque = false;
    private Tween _tween = null;

    private ScreenFader()
    {
        AddToGroup("MainScreenFade");
    }

    public void FadeToOpaque(float duration, Action onFinish = null)
    {
        if (_isOpaque) return;
        _tween?.Kill();
        _tween = CreateTween();
        _tween.Finished += EmitSignalFadeFinished;
        _tween.Finished += onFinish;
        _tween.TweenMethod(Callable.From<float>(SetFadeValue), 0f, 1f, duration);
        _isOpaque = true;
    }

    public void FadeToClear(float duration, Action onFinish = null)
    {
        if (!_isOpaque) return;
        _tween?.Kill();
        _tween = CreateTween();
        _tween.Finished += EmitSignalFadeFinished;
        _tween.Finished += onFinish;
        _tween.TweenMethod(Callable.From<float>(SetFadeValue), 1f, 0f, duration);
        _isOpaque = false;
    }

    public void SetFadeValue(float value)
    {
        Color = Color with { A = value };
    }
}
