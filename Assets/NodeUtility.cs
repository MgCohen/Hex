using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public static class NodeUtility
{
    private static Vector3Int[] evenCellNeighbours = new Vector3Int[6]
{
        new Vector3Int(1, 0),
        new Vector3Int(0, 1),
        new Vector3Int(-1, 1),
        new Vector3Int(-1, 0),
        new Vector3Int(-1, -1),
        new Vector3Int(0, -1)
};

    private static Vector3Int[] oddCellNeighbours = new Vector3Int[6]
    {
        new Vector3Int(1, 0),
        new Vector3Int(1, 1),
        new Vector3Int(0, 1),
        new Vector3Int(-1, 0),
        new Vector3Int(0, -1),
        new Vector3Int(1, -1)
    };

    public static Vector3Int[] GetNodeNeighbourIndexes(this NodeView node)
    {
        Vector3Int[] indexes = GetNeighbourIndexes(node.Index);
        Vector3Int[] Neighbours = new Vector3Int[6];
        Vector3Int nodeIndex = node.Index;

        for (int i = 0; i < indexes.Length; i++)
        {
            Neighbours[i] = new Vector3Int(indexes[i].x + nodeIndex.x, indexes[i].y + nodeIndex.y, 0);
        }
        return Neighbours;
    }

    public static Vector3Int[] GetNodeNeighbourIndexes(this NodeData node)
    {
        Vector3Int[] indexes = GetNeighbourIndexes(node.Index);
        Vector3Int[] Neighbours = new Vector3Int[6];
        Vector3Int nodeIndex = node.Index;

        for (int i = 0; i < indexes.Length; i++)
        {
            Neighbours[i] = new Vector3Int(indexes[i].x + nodeIndex.x, indexes[i].y + nodeIndex.y, 0);
        }
        return Neighbours;
    }

    public static Vector3Int[] GetNodeNeighbourIndexes(this Vector3Int nodeIndex)
    {
        Vector3Int[] indexes = GetNeighbourIndexes(nodeIndex);
        Vector3Int[] Neighbours = new Vector3Int[6];

        for (int i = 0; i < indexes.Length; i++)
        {
            Neighbours[i] = new Vector3Int(indexes[i].x + nodeIndex.x, indexes[i].y + nodeIndex.y, 0);
        }
        return Neighbours;
    }

    private static Vector3Int[] GetNeighbourIndexes(Vector3Int index)
    {
        if(index.y % 2 == 0)
        {
            return evenCellNeighbours;
        }
        else
        {
            return oddCellNeighbours;
        }
    }

    public static List<T> BroadSearch<T>(int depth, T start, Func<T, IEnumerable<T>> neighboursGetter, Func<T, bool> neighbourValidation = null, Action<T, int> onVisit = null)
    {
        HashSet<T> closedNodes = new HashSet<T>();
        List<T> openNodes = new List<T>()
        {
            start
        };

        int currentDepth = 0;
        while (openNodes.Count > 0 && currentDepth <= depth)
        {
            currentDepth++;
            for (int i = openNodes.Count - 1; i >= 0; i--)
            {
                if(neighbourValidation != null && !neighbourValidation(openNodes[i]))
                {
                    continue;
                }

                if (closedNodes.Add(openNodes[i]))
                {
                    onVisit?.Invoke(openNodes[i], currentDepth - 1);
                    var neighbours = neighboursGetter?.Invoke(openNodes[i]);
                    openNodes.AddRange(neighbours);
                }
                openNodes.Remove(openNodes[i]);
            }
        }
        return closedNodes.ToList();
    }

    public static List<T> AStartSearch<T>(T start, T end, Func<T, int> costGetter)
    {
        return null;
        //because we cash the 
    }
}