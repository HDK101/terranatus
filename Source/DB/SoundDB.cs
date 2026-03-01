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
	public AudioStreamRandomizer FireReleaseRandomizer { get; private set; }
	public AudioStreamRandomizer PickupRandomizer { get; private set; }
	public AudioStreamRandomizer PickEXPRandomizer { get; private set; }

	public override void _Ready()
	{
		SlashRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/slash_randomizer.tres");
		SwooshRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/swoosh_randomizer.tres");
		HurtRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/hurt_randomizer.tres");
		PunchRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/punch_randomizer.tres");
		EnemyDeathRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/enemy_death_randomizer.tres");
		ForwardSlashRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/forward_slash_randomizer.tres");
		FireReleaseRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Attack/fire_release_randomizer.tres");
		PickupRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/UI/pickup_randomizer.tres");
		PickEXPRandomizer = GD.Load<AudioStreamRandomizer>("res://Sounds/Misc/pick_exp_randomizer.tres");
	}
}
