using Godot;

public partial class EntitySoundPlayer : MultipleAudioPlayer2D
{
    private SoundDB soundDB;

    public override void Start()
    {
        ProcessMode = ProcessModeEnum.Always;
        soundDB = GetNode<SoundDB>("/root/SoundDB");
    }

    public void PlayAttackSound(AttackType type)
    {
        switch(type)
        {
            case AttackType.SLASH:
                PlayStream(soundDB.SlashRandomizer);
                break;
            case AttackType.PUNCH:
                PlayStream(soundDB.PunchRandomizer);
                break;
        }
    }

    public void PlayForwardSlash()
    {
        PlayStream(soundDB.ForwardSlashRandomizer);
    }

    public void PlayHurt()
    {
        PlayStream(soundDB.HurtRandomizer);
    }

    public void PlayFireRelease()
    {
        PlayStream(soundDB.FireReleaseRandomizer);
    }

    public void PlayPickEXP()
    {
        PlayStream(soundDB.PickEXPRandomizer);
    }

    public void Jump()
    {
        PlayStream(soundDB.JumpRandomizer);
    }
}