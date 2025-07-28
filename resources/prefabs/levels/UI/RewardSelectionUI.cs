using Godot;
using System;
using System.Threading.Tasks;

public partial class RewardSelectionUI : Control
{
	private EventBus _eventBus = null;

	private const int _numOfRewardBoxes = 3;
	private RewardContainer[] _rewardBoxes = new RewardContainer[_numOfRewardBoxes] { null, null, null };
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnRewardSelection += HandleOnRewardSelection;

		for(int i=0; i<_numOfRewardBoxes; i++)
		{
			_rewardBoxes[i] = GetNode<RewardContainer>($"rewards/VBoxContainer/HBoxContainer/reward_container_{i}");
		}

		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async Task HandleOnRewardSelection()
	{
		Show();
		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnRewardSelection -= HandleOnRewardSelection;
		base._ExitTree();
	}
}
