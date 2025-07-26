using Godot;
using static ActionManager;


//Action base class
public abstract partial class GameAction : Node
{
	protected Unit _creator;
	protected IMessenger _messenger;
	public Unit Creator {  get { return _creator; } }

	public virtual void Execute()
	{
	}
}
