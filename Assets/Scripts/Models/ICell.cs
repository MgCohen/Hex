using System.Collections.Generic;
using UnityEngine;

namespace Hex.Models
{
    public interface ICell
    {
        int FCost { get; }
        int GCost { get; }
        int HCost { get; }
        Vector3Int Index { get; }
        List<ICell> Neighbours { get; }
        ICell Parent { get; }
        bool Walkable { get; }

        void SetHeuristics(int newCostToNeighbour, int distance, ICell node);
        void SetNeighbours(List<ICell> neighbours);
    }
}