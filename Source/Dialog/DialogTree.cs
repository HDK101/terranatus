using Godot;
using System.Collections.Generic;

public partial class DialogTree : RefCounted
{
    public string Id { get; set; }
    public Queue<DialogContent> Nodes;
}