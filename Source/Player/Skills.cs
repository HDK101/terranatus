using System;

public class Skills(WeakReference<Player> playerRef)
{
    public Func<int, bool> HasMana { get; init; }
    public Action<int> UseMana { get; init; }

    public ForwardSlashSkill ForwardSlash { get; private set; }
    public FireballSkill Fireball { get; private set; }

    public void Start()
    {
        ForwardSlash = new(playerRef)
        {
            UseMana = UseMana,
            HasMana = HasMana,
        };
        Fireball = new(playerRef)
        {
            UseMana = UseMana,
            HasMana = HasMana,
        };
    }
}