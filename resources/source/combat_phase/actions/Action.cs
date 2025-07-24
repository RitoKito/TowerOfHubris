using Godot;
using static ActionManager;


//Action base class
public abstract partial class GameAction : Node
{
    protected Unit _authorUnit;
	protected IMessenger _messenger;
	public Unit AutorUnit {  get { return _authorUnit; } }

    public virtual void Execute()
	{
	}
}
