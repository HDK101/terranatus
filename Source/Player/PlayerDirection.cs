public class PlayerDirection
{
    public enum DirectionEnum
    {
        LEFT,
        RIGHT,
    }

    public DirectionEnum Direction { get; set; }
    public float ValueDirection
    {
        get
        {
            return Direction switch
            {
                DirectionEnum.LEFT => -1.0f,
                DirectionEnum.RIGHT => 1.0f,
                _ => 0.0f,
            };
        }
    }

    public bool Flipped()
    {
        return Direction == DirectionEnum.LEFT;
    }
}