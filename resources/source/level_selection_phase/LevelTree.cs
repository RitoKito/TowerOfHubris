using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class LevelTree : CsgBox3D
{

	private int currentDepth = 0;

	private Node3D[] _levelNodes;
	private LevelNode _rootNode;
	public Node3D RootNode { get { return _rootNode; } }

	private int _maxDepth = 4;
	private const int MAX_NODE_COUNT = 3;

	private float _yOffsetPerDepth = 7f;

	private Vector3 middleNode = new Vector3(0, 8f, -28);
	private Vector3 leftNodePos = new Vector3(-15, 8f, -28f);
	private Vector3 rightNodePos = new Vector3(15, 8f, -28f);
	private List<Vector3> nodePosition = new List<Vector3>();
	private List<LevelNode[]> tree = new List<LevelNode[]>();

	public static Dictionary<int, int[]> legalConnections = new()
	{
		{ 0, new int[] { 0, 1 } },
		{ 1, new int[] { 0, 1, 2} },
		{ 2, new int[] { 1, 2 } },
	};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		nodePosition.Add(leftNodePos);
		nodePosition.Add(middleNode);
		nodePosition.Add(rightNodePos);

		var rnd = new Random();

		GenerateTree(rnd);

		foreach (var depthLayer in tree)
		{
			foreach(var node in depthLayer)
			{
				if (node != null)
				{
					node.DrawConnectionToChild();
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// TODO Add boss nodes automatically generated
	private void GenerateTree(Random rnd)
	{
		PackedScene nodePrefab = GD.Load<PackedScene>(PathConstants.LEVEL_NODE);
		Node rootNodeInstance = nodePrefab.Instantiate();
		LevelNode rootNodeScript = rootNodeInstance as LevelNode;
		_rootNode = rootNodeScript;

		AddChild(rootNodeScript);

		rootNodeScript.GlobalPosition = middleNode;
		LevelNode[] depthZero = new LevelNode[MAX_NODE_COUNT] { null, rootNodeScript, null };
		tree.Add(depthZero);

		// Instantiates tree nodes
		for (int depth = 1; depth < _maxDepth + 1; depth++)
		{
			GenerateDepthLayer(nodePrefab, depth);
		}

		var root = tree[0][1];
		for(int depthLayer = 0; depthLayer < _maxDepth; depthLayer ++)
		{
			HashSet<(int parent, int child)> existingConnections = new HashSet<(int parent, int child)>();

			// Each nodes creates one parent-child connection
			AddRandomConnections(rnd, depthLayer, existingConnections);
			// For each node where parent count is 0, adds a parent-child connection
			// Ensures that all nodes lead to the end of the tree
			AddParentConnectionWhereNull(rnd, root, depthLayer, existingConnections);
		}

		var finalNode = nodePrefab.Instantiate() as LevelNode;
		finalNode.Depth = _maxDepth;
		AddChild(finalNode);
		finalNode.GlobalPosition = middleNode + new Vector3(0, _yOffsetPerDepth * (_maxDepth + 1), 0);

		//Temp final depth
		// TODO add boss depth to be generated every 5 depth layers
		LevelNode[] finalDepth = new LevelNode[MAX_NODE_COUNT] {null, finalNode, null };
		tree.Add(finalDepth);

		foreach(var node in tree[_maxDepth])
		{
			node.AddChild(finalNode);
			finalNode.AddParent(node);
		}
	}

	private void AddRandomConnections(Random rnd, int depthLayer, HashSet<(int parent, int child)> existingLinks)
	{
		for (int parentIndex = 0; parentIndex < MAX_NODE_COUNT; parentIndex++)
		{
			var current = tree[depthLayer][parentIndex];
			if (current == null)
				continue;

			var childIndex = legalConnections[parentIndex][rnd.Next(0, legalConnections[parentIndex].Count())];
			LevelNode next = null;

			var attempts = 0;
			var maxAttempts = 100;

			while (next == null && attempts < maxAttempts)
			{
				childIndex = legalConnections[parentIndex][rnd.Next(0, legalConnections[parentIndex].Count())];
				if (IsCross(parentIndex, childIndex, existingLinks))
				{
					childIndex = legalConnections[parentIndex][rnd.Next(0, legalConnections[parentIndex].Count())];
					attempts++;
					continue;
				}

				next = tree[depthLayer + 1][childIndex];
			}

			if(next == null)
			{
				GD.PrintErr("Failed to find random connection");
			}

			current.AddChild(next);
			next.AddParent(current);
			existingLinks.Add((parentIndex, childIndex));
		}
	}

	private void AddParentConnectionWhereNull(Random rnd, LevelNode root, int depthLayer, HashSet<(int parent, int child)> existingConnections)
	{
		for (int nodeIndex = 0; nodeIndex < MAX_NODE_COUNT; nodeIndex++)
		{
			var current = tree[depthLayer + 1][nodeIndex];
			if (current == null)
				continue;

			if (depthLayer == 0)
			{
				if(existingConnections.Contains((1, nodeIndex)))
					continue;

				root.AddChild(current);
				current.AddParent(root);
				existingConnections.Add((1, nodeIndex));
				continue;
			}



			// Version below adds any possible connections
			// that dont result in edges crossing over
			// overall making the tree more accessible
			LevelNode parent = null;
			foreach(int parentIndex in legalConnections[nodeIndex])
			{
				if(!IsCross(parentIndex, nodeIndex, existingConnections) && !existingConnections.Contains((parentIndex, nodeIndex)))
				{
					parent = tree[depthLayer][parentIndex];
					parent.AddChild(current);
					current.AddParent(parent);
					existingConnections.Add((parentIndex, nodeIndex));
				}
			}

			// Version below only adds 1 parent
			// to nodes that have no parent
			// thus yielding less connections in
			// total and making the tree less accessible

			/*if (current.GetParentCount() <= 1)
			{

				LevelNode parent = null;
				var parentIndexRef = 0;
				var parentIndex = 0;

				while (parent == null)
				{
					//parentIndex = legalConnections[nodeIndex][parentIndexRef];
					parentIndex = legalConnections[nodeIndex][rnd.Next(0, legalConnections[nodeIndex].Count())];
					if (IsCross(parentIndex, nodeIndex, existingConnections) && !existingConnections.Contains((parentIndex, nodeIndex)))
					{
						//parentIndexRef++;
						parentIndex = legalConnections[nodeIndex][rnd.Next(0, legalConnections[nodeIndex].Count())];
						continue;
					}

					parent = tree[depthLayer][parentIndex];
				}

				parent.AddChild(current);
				current.AddParent(parent);
				existingConnections.Add((parentIndex, nodeIndex));
			}*/
		}
	}

	private void GenerateDepthLayer(PackedScene nodePrefab, int depth)
	{
		var currentDepthNodes = new LevelNode[MAX_NODE_COUNT] { null, null, null };
		for (int nodeIndex = 0; nodeIndex < MAX_NODE_COUNT; nodeIndex++)
		{
			var node = nodePrefab.Instantiate() as LevelNode;
			AddChild(node);
			node.GlobalPosition = nodePosition[nodeIndex] + new Vector3(0, _yOffsetPerDepth * depth, 0);
			node.Depth = depth;
			currentDepthNodes[nodeIndex] = node;
		}
		tree.Add(currentDepthNodes);
	}

	private bool IsCross(int parentIndex, int childIndex, HashSet<(int parent, int child)> depthLinks)
	{
		return depthLinks.Any(conn =>
					(parentIndex < conn.parent && childIndex > conn.child) ||
					(parentIndex > conn.parent && childIndex < conn.child));
	}

	public void Disable()
	{
		ProcessMode = ProcessModeEnum.Disabled;
		Hide();
	}

	public void Enable()
	{
		ProcessMode = ProcessModeEnum.Always;
		Show();
	}

	// Not Used
	private int[] GetRandomDepthLayout(Random rnd)
	{
		int numberOfNodes = rnd.Next(2, 4);
		int[] positions = { 0, 0, 0 };
		if (numberOfNodes == 3)
		{
			positions = new int[3] { 1, 1, 1 };
			return positions;
		}

		var bits = new List<int> { 1, 1, 0 };
		for(int i = bits.Count; i > 0; i--)
		{
			int j = rnd.Next(0, i);
			(bits[i], bits[j]) = (bits[j], bits[i]);
		}

		positions = new int[3] { bits[0], bits[1], bits[2] };

		return positions;
	}
}
