using Godot;
using System;
using System.ComponentModel;

public partial class GameManager : Node
{
    [Signal]
    public delegate void PausedEventHandler(bool paused);

    [Export]
    private Node entities;

    [Export]
    private GameHud gameHud;

    private bool paused = false;

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            Pause();
        }
    }

    public void Pause()
    {
        paused = !paused;
        GetTree().Paused = paused;
        EmitSignal(SignalName.Paused, paused);
    }
}