using Godot;
using System;

public partial class BlackScreen : Control
{
	private Messenger _messenger;
	private AnimationPlayer _animationPlayer;
	private TransitionState _currentState;

	private const string ANIMATION_TO_BLACK = "fade_to_black";
	private const string ANIMATION_TO_NORMAL = "fade_to_normal";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_messenger = Messenger.Instance;
		_messenger.OnSceneTransition += HandleSceneTransition;


		_animationPlayer = GetNode<AnimationPlayer>("animation_player");
		_animationPlayer.AnimationFinished += HandleOnAnimationFinished;

		//SetState(CurrentState.Normal);
		SetState(TransitionState.Normal);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SetState(TransitionState state)
	{
		switch (state)
		{
			case TransitionState.Normal:
				_animationPlayer.Play(ANIMATION_TO_NORMAL);
				break;
			case TransitionState.Black:
				_animationPlayer.Play(ANIMATION_TO_BLACK);
				break;
		}

		_currentState = state;
	}

	public void HandleSceneTransition(TransitionState state)
	{
		switch(state)
		{
			case TransitionState.Normal:
				SetState(TransitionState.Normal);
				break;
			case TransitionState.Black:
				SetState(TransitionState.Black);
				break;
		}
	}


	private void HandleOnAnimationFinished(StringName animName)
	{
		/*		if (_currentState == CurrentState.Black) 
				{ 
					//_currentState = CurrentState.Black;
					_messenger.EmitFadedToBlack();
					GD.Print("Black");
				}
				else if (_currentState == CurrentState.Normal) 
				{
					//_currentState = CurrentState.Normal;
					_messenger.EmitFadedToNormal();
				}
				GD.Print("Fin");*/

		GD.Print($"{animName} Transition Concluded");
		_messenger.EmitTransitionComplete();
	}
}
