using Godot;
using System;

public partial class GameManager : Node
{
    [Export]
    private Node entities;

    [Export]
    private GameHud gameHud;

    private bool paused = false;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            paused = !paused;

            if (paused) Pause();
            else if (!paused) Resume();
        }
    }

    public void Pause()
    {
        gameHud.Menu.ShowUI();
        GetTree().Paused = true;
    }

    public void Resume()
    {
        gameHud.Menu.HideUI();
        GetTree().Paused = false;
    }
}