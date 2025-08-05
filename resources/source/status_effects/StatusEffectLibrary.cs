using Godot;
using Godot.Collections;
using System;

[GlobalClass]
public partial class StatusEffectLibrary : Resource
{
	[Export]
	public Array<PackedScene> StatusEffects = new();
}
