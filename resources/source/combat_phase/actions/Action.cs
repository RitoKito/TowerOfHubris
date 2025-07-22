using Godot;
using static ActionManager;


//Action base class
public partial interface IGameAction
{
	public virtual void Execute(ActionDelegate actionDelegate)
	{
		actionDelegate.Invoke();
	}
}
