using Godot;
using System.Collections.Generic;

public partial class DialogBox : Control
{
	[Export]
	private RichTextLabel dialogText;
	[Export]
	private TextureRect portraitRect;

	private Timer timer;

	private bool isPlaying = false;
	private string messageTarget = "";
	private string currentMessage = "";
	private int messageIndex = 0;
	private Queue<DialogContent> nodes = [];
	private DialogContent queuedToPlay = null;

	private bool active = false;

	public override void _Ready()
	{
		timer = new()
		{
			Autostart = false,
			OneShot = false,
		};
		timer.Timeout += TickMessage;
		AddChild(timer);
	}

	public PropertyTweener ShowUI()
	{
		var tween = CreateDefaultTween().SetParallel();

		portraitRect.Position = new(-160f, -30.0f);
		portraitRect.Modulate = new(Modulate, 0f);

		return tween.TweenProperty(this, "position:y", 172.0f, 0.5f);
	}

	public PropertyTweener HideUI()
	{
		var tween = CreateDefaultTween().SetParallel();

		tween.TweenProperty(portraitRect, "position", new Vector2(-127.0f, -30f), 0.75f);
		tween.TweenProperty(portraitRect, "modulate:a", 0.0f, 0.5f);

		return tween.TweenProperty(this, "position:y", 252.0f, 0.5f);
	}

	public void PlayContent(DialogContent content)
	{
		if (!active)
		{
			ShowUI().Finished += () => StartContent(content);
		}
		else
		{
			StartContent(content);
		}
	}

	public void PlayTree(DialogTree tree)
	{
		messageIndex = 0;
		dialogText.Text = "";
		portraitRect.Texture = null;
		messageTarget = "";
		dialogText.Text = "";
		currentMessage = "";
		nodes = tree.Nodes;
		PlayContent(nodes.Dequeue());
	}

	private void StartContent(DialogContent content)
	{
		currentMessage = "";
		dialogText.Text = "";
		messageIndex = 0;
		var tween = CreateDefaultTween().SetParallel();
		tween.TweenProperty(portraitRect, "position", new Vector2(0f, -30f), 0.75f);
		tween.TweenProperty(portraitRect, "modulate:a", 1.0f, 0.75f);
		portraitRect.Texture = content.Portrait;
		messageTarget = content.Message;
		isPlaying = true;
		timer.Start(0.05);
		active = true;
	}

	private Tween CreateDefaultTween()
	{
		var tween = GetTree().CreateTween().SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		return tween;
	}

	private void TickMessage()
	{
		if (isPlaying)
		{
			if (messageIndex < messageTarget.Length)
			{
				currentMessage += messageTarget[messageIndex];
				dialogText.Text = currentMessage;
				messageIndex += 1;
			}
			else
			{
				FinishOrPlayNext();
			}
		}
	}

	private void FinishOrPlayNext()
	{
		isPlaying = false;
		timer.Stop();

		if (nodes.Count > 0)
		{
			var content = nodes.Dequeue();
			GetTree().CreateTimer(2.0).Timeout += () => PlayContent(content);
			return;
		}

		GetTree().CreateTimer(2.0).Timeout += () => HideUI();
	}
}
