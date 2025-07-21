using Godot;
using static SceneManager;


//Action base class
public partial class Action : Node
{

	
	
	public virtual string Name {  get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void Execute(ActionDelegate actionDelegate)
	{
		GD.Print($"Task {Name} Completed");
		actionDelegate.Invoke();
	}
}
