using Godot;
using System;
using System.Threading.Tasks;

public partial class RewardSelectionUI : Control
{
	private EventBus _eventBus = null;

	private const int MAX_NUM_OF_RWARDS = 3;
	private RewardCard[] _rewardBoxes = new RewardCard[MAX_NUM_OF_RWARDS] { null, null, null };
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnRewardSelection += HandleOnRewardSelection;

		for(int i=0; i<MAX_NUM_OF_RWARDS; i++)
		{
			_rewardBoxes[i] = GetNode<RewardCard>($"rewards/VBoxContainer/HBoxContainer/reward_card_{i}");
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
