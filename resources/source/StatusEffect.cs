using Godot;
using System;

public abstract partial class StatusEffect : Node
{
	protected int _id = -1;
	protected virtual int Id => _id;

	protected Sprite2D _icon = null;
	protected virtual Sprite2D Icon => _icon;

	protected string _effectName = "Placeholder Blessing";
	public virtual string EffectName => _effectName;

	protected string _description = "No Description";
	public virtual string Description => _description;

	protected int _duration = 99;
	protected virtual int Duration => _duration;

	public bool IsExpired => _duration <= 0;

	protected bool _stackable = false;
	public virtual bool Stackable => _stackable;

	protected int _stackCount = 0;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void OnApply(Unit unit)
	{

	}

	public virtual void OnTurnStart(Unit unit)
	{

	}

	public virtual void OnExpire(Unit unit) 
	{ 
	
	}

	public virtual void Tick() => _duration--;

	public virtual void AddStacks(int stackCount = 1)
	{
		_stackCount++;
	}
}
