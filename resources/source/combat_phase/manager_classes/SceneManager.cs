using Godot;
using System.Collections.Generic;

public partial class SceneManager : Node3D
{
	public static SceneManager Instance { get; private set; }
	Camera3D _cameraObj;

	private readonly List<Unit> _playerUnits = new List<Unit>();
	public List<Unit> PlayerUnits { get { return _playerUnits; } }

	private bool _processingTask = false;

	public void AppendPlayerUnit(Unit unit)
	{
		_playerUnits.Add(unit);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		if (Instance == null)
			Instance = this;
		else
			Free();

		_cameraObj = GetViewport().GetCamera3D();

		InstantiatePlayerCharacters();

		if(_playerUnits.Count == 0) 
		{
			GD.Print("Warning! No player units registered in the scene!");
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void InstantiatePlayerCharacters()
	{
		PackedScene playerContainer = GD.Load<PackedScene>(PathConstants.PLAYER_CONTAINER_PATH);
		Node playerContainerInstance = playerContainer.Instantiate();
		var playerUnitContainer = playerContainerInstance as PlayerUnitContainer;
		AddChild(playerContainerInstance);

		foreach (Unit unit in playerUnitContainer.UnitArray)
		{
			if (unit != null)
			{
				AppendPlayerUnit(unit);
			}
		}
		//AppendPlayerUnit(playerContainerInstance.GetNode<Unit>("unit_pos_2/player_delta"));
	}
}
