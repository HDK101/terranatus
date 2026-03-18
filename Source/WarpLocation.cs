using Godot;

[GlobalClass]
public partial class WarpLocation : Resource
{
	[Export]
	public Levels Level { get; private set; }
	[Export]
	public int Index { get; private set; }
}
