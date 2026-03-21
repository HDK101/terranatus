using Godot;
using System;

public partial class SystemItem : Control, ISelectable
{
	public enum Button
	{
		VIDEO,
		SOUND,
		CONTROLLER,
		QUIT
	}

	private Label label;
	private TextureRect arrow;
	private Button CurrentButton { get; set; }

	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		arrow = GetNode<TextureRect>("Arrow");
	}

	private void Setup()
	{
		switch (CurrentButton)
		{
			case Button.VIDEO:
				label.Text = "Video options";
				break;
			case Button.CONTROLLER:
				label.Text = "Controller options";
				break;
			case Button.SOUND:
				label.Text = "Sounds options";
				break;
			case Button.QUIT:
				label.Text = "Quit";
				break;
		}
	}

    public void Select()
    {
        throw new NotImplementedException();
    }

    public void Unselect()
    {
        throw new NotImplementedException();
    }

    public void DoAction()
    {
        throw new NotImplementedException();
    }
}
