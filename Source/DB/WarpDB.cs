using Godot;
using System.Collections.Generic;

public partial class WarpDB : Node
{
	private Dictionary<(Levels, int), WarpItem> _locations = [];

    public override void _EnterTree()
	{
		Create(Levels.DEEP_DARK_FOREST, 0);
		Create(Levels.DEEP_DARK_FOREST, 1);
	}

	public WarpItem Retrieve(Levels level, int index)
	{
		return _locations[(level, index)];
	}

	private void Create(Levels level, int index)
	{
		_locations[(level, index)] = new(level, index);
	}
}
