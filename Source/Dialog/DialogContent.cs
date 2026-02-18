using Godot;

public partial class DialogContent: RefCounted
{
    public string Id { get; set; }
    public string Message { get; set; }
    public Texture2D Portrait { get; set; }
}