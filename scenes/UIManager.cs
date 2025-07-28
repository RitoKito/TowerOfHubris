using Godot;
using System;
using System.Threading.Tasks;

public partial class UIManager : Control
{
	private AnimationPlayer _animationPlayer = null;
	public AnimationPlayer AnimationPlayer {  get { return _animationPlayer; } }

	private const string ANIMATION_TO_BLACK = "fade_to_black";
	private const string ANIMATION_TO_NORMAL = "fade_to_normal";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("animation_player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async Task PlayAnimation(string name)
	{
		var tcs = new TaskCompletionSource<bool>();

		void OnAnimationFinished(StringName animationName)
		{
			if(name == animationName)
			{
				_animationPlayer.AnimationFinished -= OnAnimationFinished;
				tcs.SetResult(true);
			}
		}

		_animationPlayer.AnimationFinished += OnAnimationFinished;
		_animationPlayer.Play(name);

		await tcs.Task;
	}

	public async Task PlayFadeToBlack()
	{
		await PlayAnimation(ANIMATION_TO_BLACK);
	}
	public async Task PlayFadeToNormal()
	{
		await PlayAnimation(ANIMATION_TO_NORMAL);
	}
}
