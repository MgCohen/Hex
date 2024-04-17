using System.Collections.Generic;
using UnityEngine;

namespace Hex.Models
{
    public class NodeData : ICell
    {
        public NodeData(Vector3Int index, bool walkable)
        {
            Index = index;
            Walkable = walkable;
        }

        public Vector3Int Index { get; private set; }

        public bool Walkable { get; private set; }

        public List<ICell> Neighbours { get; private set; }

        public int HCost { get; private set; }
        public int GCost { get; private set; }
        public int FCost => HCost + GCost;
        public ICell Parent { get; private set; }

        public void SetNeighbours(List<ICell> neighbours)
        {
            Neighbours = neighbours;
        }

        public void SetHeuristics(int newCostToNeighbour, int distance, ICell node)
        {
            GCost = newCostToNeighbour;
            HCost = distance;
            Parent = node;
        }
    }
}
