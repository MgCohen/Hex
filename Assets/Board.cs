using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;

public class Board : Dictionary<Vector3Int, NodeData>
{
    public event Action<NodeView, InputButton> OnNodeClicked = delegate { };

    public void NodeClicked(NodeView node, InputButton button)
    {
        OnNodeClicked?.Invoke(node, button);
    }
}

public class NodeData
{
    public NodeData(Vector3Int index, bool walkable)
    {
        Index = index;
        Walkable = walkable;
    }

    public Vector3Int Index { get; private set; }

    public bool Walkable { get; private set; }

    public List<NodeData> Neighbours { get; private set; }

    public void SetNeighbours(List<NodeData> neighbours)
    {
        Neighbours = neighbours;
    }
}
