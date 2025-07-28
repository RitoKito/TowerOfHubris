using Godot;
using static ActionManager;


//Action base class
public abstract partial class GameAction : Node
{
	protected ActionManager _actionManager;
	protected Unit _creator;
	protected IEventBus _eventBus;
	public Unit Creator {  get { return _creator; } }

	public virtual void Execute()
	{
	}
}
