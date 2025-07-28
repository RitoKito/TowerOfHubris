using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class LevelTreeManager : Node3D
{
	public static LevelTreeManager Instance { get; private set; }

	private EventBus _eventBus = null;

	private LevelTree _currentTree = null;
	private IList<LevelNode> _eligibleNextNode = null;
	private LevelNode _currentLevel = null;

	private Node _currentCombatScene = null;

	private GameState _gameState = GameState.LevelTree;


	private int _escalation = 0;
	public int Escalation { get { return _escalation; } private set { } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Free();

		_eventBus = EventBus.Instance;
		_eventBus.OnMouseLeftClick += HandleMouseLeftClick;
		_eventBus.OnGameStateChanged += HandleGameStateChanged;

		_currentTree = InstantiateLevelTree() as LevelTree;
		_currentLevel = _currentTree.RootNode as LevelNode;

		_currentLevel.ShowEligibleNext();
		_currentLevel.SelectNode();
		_eligibleNextNode = _currentLevel.Children;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Node InstantiateLevel(string level)
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

	private async void HandleLevelNodeSelected(LevelNode levelNode)
	{
		_currentLevel = levelNode;
		_currentLevel.SelectNode();
		await _eventBus.EmitEnterCombat();
	}

	private void HandleMouseLeftClick(Dictionary rayCastResult)
	{
		Node colliderObj = (Node)rayCastResult["collider"];

		if (colliderObj.GetGroups().Contains(TagConst.TAG_LEVEL_NODE))
		{
			LevelNode levelNode = (LevelNode)colliderObj.GetParent();

			if (_eligibleNextNode != null && _eligibleNextNode.Contains(levelNode))
			{
                HandleLevelNodeSelected(levelNode);
			}
		}
	}

	private async void HandleGameStateChanged(GameState gameState)
	{
		if(gameState == GameState.Combat)
		{
			LoadCombatScene();
		}
		else if(gameState == GameState.LevelTree)
		{
			if(_currentCombatScene != null)
			{
				RecycleCombatScene();
			}

			_currentTree.Enable();

			if (_eligibleNextNode != null)
			{
				foreach (LevelNode unreachable in _eligibleNextNode)
				{
					if (unreachable != _currentLevel)
					{
						unreachable.SetInvalidNode();
					}
				}
			}

			_eligibleNextNode = _currentLevel.Children;

			_currentLevel.SelectNode();
			_currentLevel.ShowEligibleNext();

			await _eventBus.EmitLevelTreeLoaded();

			_escalation += 20;
		}
	}

	private void LoadCombatScene()
	{
		if (_currentCombatScene != null)
			return;

		_currentTree.Disable();
		_currentCombatScene = InstantiateLevel(PathConstants.PLACEHOLDER_LEVEL);
	}

	private void RecycleCombatScene()
	{
		_currentCombatScene.QueueFree();
		_currentCombatScene = null;
    }


}