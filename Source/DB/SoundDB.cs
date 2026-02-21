using Godot;
using System;

public partial class SoundDB : Node
{
	public AudioStreamRandomizer SlashRandomizer { get; private set; }
	public AudioStreamRandomizer SwooshRandomizer { get; private set; }
	public AudioStreamRandomizer HurtRandomizer { get; private set; }
	public AudioStreamRandomizer PunchRandomizer { get; private set; }
	public AudioStreamRandomizer EnemyDeathRandomizer { get; private set; }
	public AudioStreamRandomizer ForwardSlashRandomizer { get; private set; }

	public override void _Ready()
	{
		SlashRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/slash_randomizer.tres");
		SwooshRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/swoosh_randomizer.tres");
		HurtRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/hurt_randomizer.tres");
		PunchRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/punch_randomizer.tres");
		EnemyDeathRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/enemy_death_randomizer.tres");
		ForwardSlashRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/forward_slash_randomizer.tres");
	}
}
