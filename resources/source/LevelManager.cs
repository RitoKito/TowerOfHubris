using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelManager : Node3D
{
	public static LevelManager Instance { get; private set; }

	private Messenger _messenger = null;

	private LevelTree _currentTree = null;
	private IList<LevelNode> _eligibleNextNode = null;
	private LevelNode _currentLevel = null;

	private Node _currentCombatScene = null;


	private int _escalation = 0;
	public int Escalation { get { return _escalation; } private set { } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Free();

		_messenger = Messenger.Instance;
		_messenger.OnMouseLeftClick += HandleMouseLeftClick;
		_messenger.OnCombatSceneConcluded += HandleCombatSceneConcluded;

		_currentTree = InstantiateLevelTree() as LevelTree;
		_currentLevel = _currentTree.RootNode as LevelNode;


		HandleLevelNodeSelected(_currentLevel);
		//InstantiateLevel(PathConstants.PLACEHOLDER_LEVEL);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Node InstantiateLevel(String level)
	{
		PackedScene packedScene = GD.Load<PackedScene>(level);
		Node levelNode = packedScene.Instantiate() as Node3D;
		AddChild(levelNode);

		return levelNode;
	}

	private Node InstantiateLevelTree()
	{
		PackedScene packedScene = GD.Load<PackedScene>(PathConstants.LEVEL_TREE_PLACEHOLDER);
		Node levelTree = packedScene.Instantiate() as Node3D;
		AddChild(levelTree);

		return levelTree;
	}

	private void HandleLevelNodeSelected(LevelNode levelNode)
	{
		_currentLevel = levelNode;
		GD.Print(
		_currentLevel.Children.Count());

		if (_eligibleNextNode != null)
		{
			foreach (LevelNode unreachable in _eligibleNextNode)
			{
				if (unreachable != _currentLevel)
				{
					unreachable.SetInvalidLevel();
				}
			}
		}

		_eligibleNextNode = levelNode.Children;

		levelNode.SelectLevel();

		// Transition to Combat
		if(_currentLevel.Depth > 0)
		{
			_messenger.EmitGameStateChanged(GameState.Combat);
			LoadCombatScene();
		}
	}

	private void HandleMouseLeftClick(Dictionary rayCastResult)
	{
		Node colliderObj = (Node)rayCastResult["collider"];

		if (colliderObj.GetGroups().Contains(TagConst.TAG_LEVEL_NODE))
		{
			LevelNode levelNode = (LevelNode)colliderObj.GetParent();

			if(_eligibleNextNode.Contains(levelNode))
				HandleLevelNodeSelected(levelNode);
		}
	}

	private void HandleCombatSceneConcluded()
	{
		_messenger.EmitGameStateChanged(GameState.LevelTree);
		RecycleCombatScene();
		_currentTree.Enable();
	}

	private void LoadCombatScene()
	{
		_currentTree.Disable();

	   _currentCombatScene = InstantiateLevel(PathConstants.PLACEHOLDER_LEVEL);
	}

	private void RecycleCombatScene()
	{
		_currentCombatScene.QueueFree();
	}
}
