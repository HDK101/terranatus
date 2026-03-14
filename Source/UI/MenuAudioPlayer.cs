using Godot;
using System;

public partial class MenuAudioPlayer : Node
{	
	private AudioStreamPlayer audioStreamPlayer;
    private SoundDB soundDB;

	public override void _Ready()
	{
        soundDB = GetNode<SoundDB>("/root/SoundDB");
        audioStreamPlayer = new()
        {
            Stream = new AudioStreamPolyphonic(),
            Bus = "SFX"
        };

		AddChild(audioStreamPlayer);
        audioStreamPlayer.Play();
    }

    public void PlayAccept()
    {
        GetPlaybackPolyphonic().PlayStream(soundDB.UIAccept);
    }

    public void PlayChange()
    {
        GetPlaybackPolyphonic().PlayStream(soundDB.UIChange);
    }

    public void PlayPause()
    {
        GetPlaybackPolyphonic().PlayStream(soundDB.UIPause);
    }

    private AudioStreamPlaybackPolyphonic GetPlaybackPolyphonic()
    {
        return audioStreamPlayer.GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
    }
}
