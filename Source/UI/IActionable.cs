using System;

public interface IActionable
{
	Action Action { get; set; }
	void Execute();
}
