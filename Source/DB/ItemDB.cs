using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public partial class ItemDB : Node
{
    public ReadOnlyDictionary<string, ItemBlueprint> Items => _items.AsReadOnly();
    private readonly Dictionary<string, ItemBlueprint> _items = [];

    private readonly string BASE_PATH = "res://Resources/Items";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var dir = DirAccess.Open(BASE_PATH);

        dir.ListDirBegin();

        string fileName = dir.GetNext();

        while (fileName != "")
        {
            LoadFromPath(BASE_PATH + "/" + fileName);
            fileName = dir.GetNext();
        }
    }

    public ItemBlueprint Retrieve(string id)
    {
        return _items.GetValueOrDefault(id, null);
    }

    private void LoadFromPath(string path)
    {
        ItemBlueprint blueprint = GD.Load<ItemBlueprint>(path);
        _items.Add(blueprint.Id, blueprint);
    }
}