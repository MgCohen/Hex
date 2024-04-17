using System.Collections.Generic;
using UnityEngine;
using Hex.Models;

namespace Hex.Controllers
{
    interface IPathFinder

    {
        IList<ICell> IFindPathOnMap(ICell cellStart, ICell cellEnd, IMap map);

    }

    public class BoardNavigation : IPathFinder
    {
        private PathCache cachedPaths = new PathCache();
        public IList<ICell> IFindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
        {
            if (cachedPaths.TryGetPath(cellStart, cellEnd, out List<ICell> path))
            {
                return path;
            }

            if (AStarPath(cellStart, cellEnd, out path))
            {
                cachedPaths.CachePath(path);
            }

            return path;
        }

        private bool AStarPath(ICell start, ICell end, out List<ICell> path)
        {
            ICell startNode = start;
            ICell targetNode = end;

            List<ICell> openSet = new List<ICell>();
            HashSet<ICell> closedSet = new HashSet<ICell>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                ICell node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost)
                    {
                        if (openSet[i].HCost < node.HCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    path = RetracePath(startNode, targetNode);
                    return true;
                }

                foreach (ICell neighbour in node.Neighbours)
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.GCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                    {
                        int distance = GetDistance(neighbour, targetNode);
                        neighbour.SetHeuristics(newCostToNeighbour, distance, node);
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }

            path = new List<ICell>();
            return false;
        }

        private List<ICell> RetracePath(ICell startNode, ICell endNode)
        {
            List<ICell> path = new List<ICell>()
        {
            endNode,
        };
            ICell currentNode = endNode;

            while (currentNode != startNode)
            {
                currentNode = currentNode.Parent;
                path.Add(currentNode);
            }
            path.Reverse();

            return path;
        }

        private int GetDistance(ICell nodeA, ICell nodeB)
        {
            return Mathf.RoundToInt(Vector3Int.Distance(nodeA.Index, nodeB.Index));
        }

    }
}
