using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public partial class RewardGenerator : Node3D
{
	private EventBus _eventBus = null;
	private List<StatusEffect> _rewardPool = new List<StatusEffect>();
	private Random _rnd = new Random();

	private const int REWARD_COUNT = 3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_eventBus = EventBus.Instance;
		_eventBus.OnGameStateChanged += HandleGameStateChanged;

		LoadRewardPool();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void LoadRewardPool()
	{
		var dir = DirAccess.Open(PathConstants.STATUS_EFFECT_OBJS);

		if(dir == null)
		{
			GD.PrintErr("Could not locate Status Effect folder");
			return;
		}

		dir.ListDirBegin();
		string statusEffectName = dir.GetNext();

		while(!string.IsNullOrEmpty(statusEffectName))
		{
			if(!dir.CurrentIsDir() && statusEffectName.EndsWith(".tscn"))
			{
				string path = $"{PathConstants.STATUS_EFFECT_OBJS}/{statusEffectName}";
				PackedScene packedScene = ResourceLoader.Load<PackedScene>(path);

				if(packedScene != null)
				{
					StatusEffect statusEffect = packedScene.Instantiate() as StatusEffect;
					_rewardPool.Add(statusEffect);
				}
			}

			statusEffectName = dir.GetNext();
		}

		dir.ListDirEnd();
	}

	private async Task HandleGameStateChanged(GameState state)
	{
		switch(state)
		{
			case GameState.Combat:
				List<StatusEffect> rewards = new List<StatusEffect>();
				List<int> rewardIndx = Utils.GetThreeUniqueRandomNumbers(0, _rewardPool.Count);
				for(int i = 0; i < REWARD_COUNT; i++) {
					rewards.Add(_rewardPool[rewardIndx[i]]);
				}

				await _eventBus.EmitAssignRewards(rewards);
				break;
		}

		await Task.Yield();
	}

	public override void _ExitTree()
	{
		_eventBus.OnGameStateChanged -= HandleGameStateChanged;
		base._ExitTree();
	}
}
