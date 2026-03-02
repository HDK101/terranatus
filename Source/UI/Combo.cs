using Godot;

public partial class Combo : Control
{
    [Export]
    private ComboNumbers comboNumbers;

    private Timer timeout = new()
    {
        OneShot = true,
        Autostart = false,
    };

    private int currentNumber = 0;
    private bool active = false;

    public void Add()
    {
        if (!active)
        {
            var tween = CreateDefaultTween();
            tween.TweenProperty(this, "position:x", 0.0f, 0.5f);
            active = true;
        }
        currentNumber += 1;
        comboNumbers.Text = currentNumber.ToString();
        timeout.Start(5.0);
    }

    public void OnTimeout()
    {
        var tween = CreateDefaultTween();
        tween.TweenProperty(this, "position:x", -64.0f, 0.5f);
        currentNumber = 0;
        active = false;
    }

    public override void _Ready()
    {
        timeout.Timeout += OnTimeout;
        AddChild(timeout);
    }

    private Tween CreateDefaultTween()
    {
        var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
        tween.SetPauseMode(Tween.TweenPauseMode.Process);
        return tween;
    }
}