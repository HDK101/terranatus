using Godot;

public static class MenuElementUtils
{
    public static Tween SlideIn(Control control)
    {
        Tween tween = control.CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
        var size = control.GetViewport().GetVisibleRect().Size;

        control.Position = new(0.0f, size.Y);
        control.Modulate = new(1.0f, 1.0f, 1.0f, 0.0f);

        tween.TweenProperty(control, "modulate:a", 1.0f, 0.5f);
        tween.TweenProperty(control, "position:y", 0.0f, 0.5f);

        return tween;
    }

    public static Tween SlideOut(Control control)
    {
        Tween tween = control.CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();
        var size = control.GetViewport().GetVisibleRect().Size;

        tween.TweenProperty(control, "modulate:a", 0.0f, 0.5f);
        tween.TweenProperty(control, "position:y", -size.Y, 0.5f);
        tween.Chain().TweenCallback(Callable.From(control.Hide));

        return tween;
    }

    public static Tween FadeIn(Control control, float secondsDuration = 0.3f)
    {
        control.Show();
        Tween tween = control.CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();

        control.Modulate = new(1.0f, 1.0f, 1.0f, 0.0f);

        tween.TweenProperty(control, "modulate:a", 1.0f, secondsDuration);

        return tween;
    }

    public static Tween FadeOut(Control control)
    {
        Tween tween = control.CreateTween().SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Cubic).SetParallel();

        tween.TweenProperty(control, "modulate:a", 0.0f, 0.3f);
        tween.Chain().TweenCallback(Callable.From(control.Hide));

        return tween;
    }
}