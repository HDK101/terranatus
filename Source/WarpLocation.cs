using Godot;
using System;

[GlobalClass]
public partial class WarpLocation : Resource
{
	[Export]
	public Levels Level { get; private set; }
	[Export]
	public int Index { get; private set; }

	public bool IsSameWarp(WarpItem warpItem)
	{
		return Level == warpItem.Level && Index == warpItem.Index;
	}
}
