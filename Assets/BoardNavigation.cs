using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardNavigation
{

    public BoardNavigation(BoardManager manager)
    {
        this.manager = manager;

        manager.OnNodeSelectionChanged += OnNodeSelectionChanged;
    }

    private BoardManager manager;
    private NodeData startNode;
    private NodeData endNode;
    private PathCache cachedPaths = new PathCache();

    private Dictionary<NodeData, int> nodeCosts = new Dictionary<NodeData, int>();


    private void OnNodeSelectionChanged(NodeData data, bool selected, PointerEventData pointerData)
    {
        if(pointerData == null)
        {
            //if there is no pointer data, return as there is no context
            return;
        }

        if (!selected)
        {
            if (data == startNode || data == endNode)
            {
                //if we are deselecting a currently used node, lets change to null the used nodes
                data = null;
            }
            else
            {
                //if we are deselecting and its neither the start or end node, we dont care about it
                return;
            }
        }

        if (pointerData.button == PointerEventData.InputButton.Left)
        {
            SetStartNode(data);
        }
        else
        {
            SetEndNode(data);
        }
    }

    public List<NodeData> SetStartNode(NodeData node)
    {
        SwapSelection(ref startNode, node);
        //calculate all values
        NodeUtility.BroadSearch(int.MinValue, node, (n) => n.Neighbours, null, (n, i) =>
        {
            if(nodeCosts.TryGetValue(n, out int cost) && cost <= i)
            {
                return;
            }
            nodeCosts[n] = i;
        });
        return CalculatePath(startNode, endNode);
    }

    public List<NodeData> SetEndNode(NodeData node)
    {
        SwapSelection(ref endNode, node);
        return CalculatePath(startNode, endNode);
    }

    private void SwapSelection(ref NodeData currentValue, NodeData newValue)
    {
        var oldValue = currentValue;
        currentValue = newValue;
        if(oldValue != null)
        {
            manager.ToggleNodeSelection(oldValue, null);
        }
    }

    private List<NodeData> CalculatePath(NodeData start, NodeData end)
    {
        if(startNode == null || endNode == null)
        {
            return null;
        }

        if(cachedPaths.TryGetPath(start, end, out List<NodeData> path))
        {
            return path;
        }

        path = AStarPath(start, end);
        return path;
    }

    private List<NodeData> AStarPath(NodeData start, NodeData end)
    {
        return null;
    }

}

public class PathCache: Dictionary<NodeData, Dictionary<NodeData, List<NodeData>>>
{
    public bool TryGetPath(NodeData start, NodeData end, out List<NodeData> path)
    {
        if(TryGetValue(start, out var paths))
        {
            return paths.TryGetValue(end, out path);
        }
        path = null;
        return false;
    }

    public void CachePath(List<NodeData> path)
    {
        NodeData start = path[0];
        NodeData end = path[^1];

        if(!TryGetValue(start, out var paths))
        {
            paths = new Dictionary<NodeData, List<NodeData>>();
            Add(start, paths);
        }
        paths[end] = path;

        if(!TryGetValue(end, out paths))
        {
            paths = new Dictionary<NodeData, List<NodeData>>();
            Add(end, paths);
        }

        path.Reverse();
        paths[start] = path;

    }
}
