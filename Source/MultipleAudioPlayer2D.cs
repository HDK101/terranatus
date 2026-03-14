using Godot;

public abstract partial class MultipleAudioPlayer2D : AudioStreamPlayer2D
{
    private AudioStream change;
    private AudioStream accept;
    private AudioStream pause;

    public override void _Ready()
    {
        Stream = new AudioStreamPolyphonic()
        {
            Polyphony = 32,
        };
        Start();
        Play();
    }

    public abstract void Start();

    public void PlayStream(AudioStream stream)
    {
        GetPlaybackPolyphonic()?.PlayStream(stream);
    }

    private AudioStreamPlaybackPolyphonic GetPlaybackPolyphonic()
    {
        return GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
    }
}