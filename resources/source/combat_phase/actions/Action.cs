using Godot;
using static ActionManager;


//Action base class
public abstract partial class GameAction : Node
{
    protected Unit _authorUnit;
	public Unit AutorUnit {  get { return _authorUnit; } }

    public virtual void Execute(ActionDelegate actionDelegate)
	{
		actionDelegate.Invoke();
	}
}
