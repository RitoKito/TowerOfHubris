using Godot;
using System;

public partial class RewardSelectionUI : Control
{
	private Messenger _messenger = null;

	private const int _numOfRewardBoxes = 3;
	private RewardContainer[] _rewardBoxes = new RewardContainer[_numOfRewardBoxes] { null, null, null };
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;
		_messenger.OnRewardScreen += HandleRewardScreen;

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

	public void HandleRewardScreen()
	{
		Show();
	}

	public override void _ExitTree()
	{
		_messenger.OnRewardScreen -= HandleRewardScreen;
		base._ExitTree();
	}
}
