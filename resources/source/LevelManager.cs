using Godot;
using System;

public partial class LevelManager : Node3D
{
    public static LevelManager Instance { get; private set; }

    private Node3D _currentLevel = null;
    private int _escalation = 0;
    public int Escalation { get { return _escalation; } private set { } }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Free();


        InstantiateLevel(PathConstants.PLACEHOLDER_LEVEL);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void InstantiateLevel(String level)
    {
        PackedScene packedScene = GD.Load<PackedScene>(level);
        Node levelNode = packedScene.Instantiate() as Node3D;
        AddChild(levelNode);
    }
}
