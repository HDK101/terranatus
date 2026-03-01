using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class DialogDB: Node
{
    private readonly string BASE_PATH = "res://Resources/Dialogs";
    private readonly string BASE_CONTENT_PATH = "res://Resources/Dialogs/Content";
    private readonly string BASE_TREE_PATH = "res://Resources/Dialogs/Tree";
    private Godot.Collections.Dictionary<string, Texture2D> portraits = [];
    private Godot.Collections.Dictionary<string, DialogContent> contents = [];
    private Godot.Collections.Dictionary<string, DialogTree> trees = [];


    public override void _Ready()
    {
        portraits.Add("AEMILIA_NAKED_DEFAULT", GD.Load<Texture2D>("res://Sprites/Portrait/aemilia_34_view_portrait.png"));

        LoadContents();
        LoadTrees();
    }

    private void LoadContents()
    {
        var dir = DirAccess.Open(BASE_CONTENT_PATH);

        dir.ListDirBegin();

        string fileName = dir.GetNext();

        while (fileName != "")
        {
            LoadContentFromPath(BASE_CONTENT_PATH + "/" + fileName);
            fileName = dir.GetNext();
        }
    }

    private void LoadTrees()
    {
        var dir = DirAccess.Open(BASE_TREE_PATH);

        dir.ListDirBegin();

        string fileName = dir.GetNext();

        while (fileName != "")
        {
            LoadTreeFromPath(BASE_TREE_PATH + "/" + fileName);
            fileName = dir.GetNext();
        }
    }

    private void LoadContentFromPath(string v)
    {
        FileAccess fileContent = FileAccess.Open(v, FileAccess.ModeFlags.Read);
        var contentJson = Json.ParseString(fileContent.GetAsText()).AsGodotDictionary();
        Texture2D portrait = portraits[contentJson["portrait"].AsString()];
        var content = new DialogContent()
        {
            Id = contentJson["id"].AsString(),
            Message = contentJson["content"].AsGodotDictionary()["en_US"].AsString(),
            Portrait = portrait,
        };
        contents.Add(content.Id, content);
    }

    private void LoadTreeFromPath(string v)
    {
        GD.Print(v);
        FileAccess fileContent = FileAccess.Open(v, FileAccess.ModeFlags.Read);
        var treeJson = Json.ParseString(fileContent.GetAsText()).AsGodotDictionary();
        GD.Print(treeJson);
        string[] nodeIds = treeJson["nodes"].AsStringArray();

        Queue<DialogContent> stackNodes = [];

        foreach (string nodeId in nodeIds)
        {
            stackNodes.Enqueue(contents[nodeId]);
        }

        var tree = new DialogTree()
        {
            Id = treeJson["id"].AsString(),
            Nodes = stackNodes,
        };
        trees.Add(tree.Id, tree);
    }

    public Texture2D RetrievePortrait(string id)
    {
        return portraits[id];
    }

    public DialogContent RetrieveContent(string id)
    {
        return contents[id];
    }

    public DialogTree RetrieveTree(string id)
    {
        return trees[id];
    }
}