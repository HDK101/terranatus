using Godot;
using System;

public partial class GameStats : Node
{
	public WarpItem LastWarp { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		WarpDB warpDB = GetNode<WarpDB>("/root/WarpDB");
		LastWarp = warpDB.Retrieve(Levels.DEEP_DARK_FOREST, 0);
		GD.Print(LastWarp);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
