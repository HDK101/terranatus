using Godot;

public partial class GameHudAudioPlayer : AudioStreamPlayer
{
	private AudioStream change;
	private AudioStream accept;
	private AudioStream pause;

	public override void _Ready()
	{
		Stream = new AudioStreamPolyphonic();

		change = GD.Load<AudioStream>("res://Sounds/UI/change.wav");
		accept = GD.Load<AudioStream>("res://Sounds/UI/accept.wav");
		pause = GD.Load<AudioStream>("res://Sounds/UI/pause.wav");

		Play();
	}

	public void PlayAccept()
	{
		GetPlaybackPolyphonic().PlayStream(accept);
	}

	public void PlayChange()
	{
		GetPlaybackPolyphonic().PlayStream(change);
	}

	public void PlayPause()
	{
		GetPlaybackPolyphonic().PlayStream(pause);
	}

	private AudioStreamPlaybackPolyphonic GetPlaybackPolyphonic()
	{
		return GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
	}
}
