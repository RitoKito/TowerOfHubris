using Godot;
using System;
using System.Collections.Generic;

public partial class LevelNode : Node3D
{

	private int _depth;
	public int Depth { get { return _depth; } set { _depth = value; } }

	private const int MAX_CONNECTIONS = 3;

	private List<LevelNode> _children = new List<LevelNode>();
	public IList<LevelNode> Children { get { return _children.AsReadOnly(); } }

	private List<LevelNode> _parents = new List<LevelNode>();

	private CsgBox3D nodeObj = null;
	// A list of curves that draws the connecting line between nodes
	public NodeLine[] NodeLine = new NodeLine[MAX_CONNECTIONS] {null, null, null};

	private Material _birghtRed;
	private Material _defaultMaterial;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nodeObj = GetNode<CsgBox3D>("node_object");

		_birghtRed = nodeObj.Material;
		_defaultMaterial = nodeObj.Material;

		for (int i = 0; i < MAX_CONNECTIONS; i++)
		{
			NodeLine[i] = GetNode($"node_object/path{i}") as NodeLine;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddChild(LevelNode node)
	{
		_children.Add(node);
	}

	public void AddParent(LevelNode node)
	{
		_parents.Add(node);
	}

	public int GetParentCount()
	{
		return _parents.Count;
	}

	public void DrawConnectionToChild()
	{
		int i = 0;
		foreach (LevelNode childNode in _children)
		{
			NodeLine[i].SetPoint(childNode.GlobalPosition);
			i++;
		}
	}

	public void ChangeMaterial(Material material)
	{
		nodeObj.Material = material;
	}

	public void SetNewMaterial(Material material)
	{
		_defaultMaterial = material;
		nodeObj.Material = material;
	}

	public void SelectNode()
	{
		StandardMaterial3D mat = new StandardMaterial3D();
		mat.AlbedoColor = new Color(0, 1, 1);
		SetNewMaterial(mat);

		/*foreach (LevelNode node in _children)
		{
			node.SetNextLevel();
		}*/
	}

	public void ShowEligibleNext()
	{
		foreach (LevelNode node in _children)
		{
			node.ShowEligiblePath();
		}
	}

	public void ShowEligiblePath()
	{
		StandardMaterial3D mat = new StandardMaterial3D();
		mat.AlbedoColor = new Color(0, 1, 0);
		SetNewMaterial(mat);

		foreach (LevelNode childNode in _children)
		{
			childNode.SetValidFutureLevel();
		}
	}

	public void SetValidFutureLevel()
	{
		SetNewMaterial(_birghtRed);

		foreach (LevelNode childNode in _children)
		{
			childNode.SetValidFutureLevel();
		}
	}


	public void SetInvalidNode()
	{
		StandardMaterial3D mat = new StandardMaterial3D();
		mat.AlbedoColor = new Color(0.5f, 0.5f, 0.5f);
		_defaultMaterial = mat;
		nodeObj.Material = mat;

		foreach(LevelNode childNode in _children)
		{
			childNode.SetInvalidNode();
		}
	}

	public void _on_node_collider_mouse_entered()
	{
		var material = new StandardMaterial3D();
		material.AlbedoColor = new Color(1, 0, 1f);
		nodeObj.Material = material;
	}

	public void _on_node_collider_mouse_exited()
	{
		nodeObj.Material = _defaultMaterial;
	}
}
