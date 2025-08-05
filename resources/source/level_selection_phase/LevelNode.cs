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

	private Sprite3D _nodeSprite = null;
	// A list of curves that draws the connecting line between nodes
	public NodeLine[] NodeLines = new NodeLine[MAX_CONNECTIONS] {null, null, null};

	private bool _isExtreme = false;
	public bool IsExtreme { get { return _isExtreme; } set { _isExtreme=value; } }

	private bool _isValid = true;

	private Color _blue = new Color(103/255f, 148/255f, 255/255f);
	private Color _green = new Color(101/255f, 159/255f, 61/255f);
	private Color _grey = new Color(69/255f, 65/255f, 67/255f);
	private Color _yellow = new Color(245/255f, 221/255f, 0/255f);
	private Color _red = new Color(255/255f, 0/255f, 0/255f);
	private Color _defaultColor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_nodeSprite = GetNode<Sprite3D>("node_sprite");

		for (int i = 0; i < MAX_CONNECTIONS; i++)
		{
			NodeLines[i] = GetNode($"node_sprite/path{i}") as NodeLine;
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
			NodeLines[i].SetPoint(childNode.GlobalPosition);
			i++;
		}
	}

	public void SelectNode()
	{
		_isValid = false;
		_nodeSprite.Modulate = _blue;
		_defaultColor = _blue;

		// set root lines that do not connect
		// to grey colour
		if (_parents.Count > 0 && _parents[0].Depth == 0) 
		{
			for(int i = 0; i < MAX_CONNECTIONS; i++)
			{
				if (_parents[0].Children[i] != this)
				{
					_parents[0].NodeLines[i].SetLineColour(_grey);
				}
			}
		}
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
		_isValid = true;
		_nodeSprite.Modulate = _green;
		_defaultColor = _green;

		/*foreach (LevelNode childNode in _children)
		{
			childNode.SetValidFutureLevel();
		}*/

		for (int i = 0; i < MAX_CONNECTIONS; i++)
		{

			if (i < NodeLines.Length)
				NodeLines[i].SetDefaultLineColour();

			if (i < Children.Count)
				_children[i].SetValidFutureLevel();
		}
	}

	public void SetValidFutureLevel()
	{
		_isValid = true;
		_nodeSprite.Modulate = _yellow;
		_defaultColor = _yellow;

		/*foreach (LevelNode childNode in _children)
		{
			childNode.SetValidFutureLevel();
		}*/

		for (int i = 0; i < MAX_CONNECTIONS; i++)
		{

			if (i < NodeLines.Length)
				NodeLines[i].SetDefaultLineColour();

			if (i < Children.Count)
				_children[i].SetValidFutureLevel();
		}
	}

	public void SetInvalidNode(LevelNode previousNode, LevelNode currentNode)
	{
		_isValid = false;
		_nodeSprite.Modulate = _grey;
		_defaultColor = _grey;

		/*foreach (LevelNode childNode in _children)
		{
			childNode.SetInvalidNode();
		}*/

		for(int i = 0; i < MAX_CONNECTIONS; i++)
		{
			if (previousNode != null && i < previousNode.Children.Count)
			{
				if (previousNode.Children[i] != currentNode)
					previousNode.NodeLines[i].SetLineColour(_grey);
			}

			if (i < NodeLines.Length)
				NodeLines[i].SetLineColour(_grey);

			if (i < Children.Count)
				_children[i].SetInvalidNode(this, _children[i]);
		}
	}

	public void SetFirst()
	{
		_nodeSprite.Texture = GD.Load<Texture2D>(PathConstants.SPRITE_NODE_START);
	}

	public void SetExtreme()
	{
		_isExtreme = true;
		_nodeSprite.Texture = GD.Load<Texture2D>(PathConstants.SPRITE_NODE_EXTREME);
	}

	public void _on_node_collider_mouse_entered()
	{
		if(_isValid)
			_nodeSprite.Modulate = _red;
	}

	public void _on_node_collider_mouse_exited()
	{
		_nodeSprite.Modulate = _defaultColor;
	}
}
