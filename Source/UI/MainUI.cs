using Godot;

public partial class MainUI : Control, MenuElement
{
    public void HideElement()
	{
		MenuElementUtils.FadeOut(this);
	}

    public void ShowElement()
    {
		MenuElementUtils.FadeIn(this);
    }

	public void OnPause(bool paused)
    {
        if (paused) {
			HideElement();
            return;
        }
		ShowElement();
    }
}
