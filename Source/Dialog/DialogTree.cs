using System.Collections.Generic;
using Godot;

public partial class DialogTree: RefCounted
{
    public string Id { get; set; }
    public Queue<DialogContent> Nodes;
}