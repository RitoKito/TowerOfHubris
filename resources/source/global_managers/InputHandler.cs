using Godot;
using Godot.Collections;

public partial class InputHandler : Node3D
{

	// TODO UI BUS
	public static InputHandler Instance { get; private set; }

	private EventBus _eventBus;
	private Camera3D _cameraObj;

	private bool _inputEnabled = true;

	private bool _holdingLeftMouse = false;
	public bool HoldingLeftMouse { get { return _holdingLeftMouse; } private set { } }

	private bool _clickedOnCollider = false;
	public bool ClickedOnCollider { get { return _clickedOnCollider; } private set { } }

	private Dictionary _objectFromLeftClick = null;
	public Dictionary ObjectFromLeftClick {  get { return _objectFromLeftClick; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		_eventBus = EventBus.Instance;
		_cameraObj = GetViewport().GetCamera3D();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	   if(!_inputEnabled) 
		{
			_holdingLeftMouse = false;
			_clickedOnCollider = false;
			_objectFromLeftClick = null;
			return; 
		}

	   if (Input.IsActionJustPressed("mouse_left"))
		{
			_objectFromLeftClick = Utils.ShootRayCast(_cameraObj);
			
			if(_objectFromLeftClick != null )
			{
				_clickedOnCollider = true;
				_eventBus.EmitMouseLeftClicked(_objectFromLeftClick);
			}
		}

		if (Input.IsMouseButtonPressed(MouseButton.Left)) 
		{
			_holdingLeftMouse = true;

			//If player clicked on collider object
			//and is holding left mouse
			if(_objectFromLeftClick != null)
			{
				_clickedOnCollider = true;
			}
			else
			{
				_clickedOnCollider = false;
			}
		}

		if (Input.IsActionJustReleased("mouse_left"))
		{
			_holdingLeftMouse = false;
			_clickedOnCollider = false;
			_objectFromLeftClick = null;
			Dictionary objectFromMouseLeftRelease = Utils.ShootRayCast(_cameraObj);
			_eventBus.EmitMouseLeftReleased(objectFromMouseLeftRelease);
		}
	}

	// TODO potentially layer combat logic over general logic
	private void HandleTurnStateChanged(TurnState state)
	{
		switch (state)
		{
			case TurnState.PlayerTurn:
				_inputEnabled = true;
				break;
			case TurnState.InProgress:
				_inputEnabled = false;
				break;
			case TurnState.SceneComplete:
				_inputEnabled = true;
				break;
			default:
				_inputEnabled = true;
				break;
			
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}
}
