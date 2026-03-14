using Godot;
using System;

public partial class PlayerView(AnimationTree animationTree, Sprite2D sprite) : RefCounted
{
    public bool FlipH { get => sprite.FlipH; set => sprite.FlipH = value; }
    public Vector2 SpriteOffset { get => sprite.Offset; set => sprite.Offset = value; }

    private AnimationNodeStateMachinePlayback stateMachine;

    public void StartIdle()
    {
        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/StateMachine/playback");
        stateMachine.Start("Idle");
    }

    public void StartSleeping()
    {
        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/StateMachine/playback");
        stateMachine.Start("Sleeping");
    }

    public void Dying()
    {
        stateMachine.Travel("Dying");
    }

    public void Attack()
    {
        stateMachine.Travel("AttackSword");
    }

    public void Idle()
    {
        stateMachine.Travel("Idle");
    }

    public void Walk()
    {
        stateMachine.Travel("Walk");
    }

    public void Respawning()
    {
        stateMachine.Travel("Respawning");
    }

    public void Jump()
    {
        stateMachine.Travel("Jump");
    }

    public void Fall()
    {
        stateMachine.Travel("Fall");
    }

    public void HideCharacter()
    {
        sprite.Hide();
    }
}