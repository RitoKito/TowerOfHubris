using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class RewardSelectionUI : Control
{
	private EventBus _eventBus = null;
	private AnimationPlayer _animationPlayer = null;

	private const int MAX_NUM_OF_RWARDS = 3;
	private RewardCard[] _rewardCards = new RewardCard[MAX_NUM_OF_RWARDS] { null, null, null };
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnRewardSelection += HandleOnRewardSelection;
		_eventBus.OnAssignRewards += HandleAssignRewards;

		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		for(int i=0; i<MAX_NUM_OF_RWARDS; i++)
		{
			_rewardCards[i] = GetNode<RewardCard>($"rewards/VBoxContainer/HBoxContainer/reward_card_{i}");
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
		_animationPlayer.Play("rewards_appear");
		await Task.Yield();
	}

	public async Task HandleAssignRewards(List<StatusEffect> rewards)
	{
		for (int i = 0; i < rewards.Count; i++)
		{
			_rewardCards[i].SetReward(rewards[i]);
		}

		await Task.Yield();
	}


	public override void _ExitTree()
	{
		_eventBus.OnRewardSelection -= HandleOnRewardSelection;
		_eventBus.OnAssignRewards -= HandleAssignRewards;
		base._ExitTree();
	}
}
