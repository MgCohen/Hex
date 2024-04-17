using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hex.Models;

namespace Hex.Controllers
{
    public class BoardBuilder
    {
        public Board CreateBoard(int size)
        {
            Board board = new Board();
            NodeUtility.BroadSearch<Vector3Int>(size, Vector3Int.zero, (v) => v.GetNodeNeighbourIndexes(), null, (v, i) => board.Add(v, CreateNode(v)));

            foreach (var node in board)
            {
                Vector3Int[] neightoubrIndexes = node.Key.GetNodeNeighbourIndexes();
                List<ICell> neighbourList = neightoubrIndexes.Where(index => board.ContainsKey(index) && board[index].Walkable).Select(index => board[index]).ToList();
                node.Value.SetNeighbours(neighbourList);
            }

            return board;
        }

        private ICell CreateNode(Vector3Int index)
        {
            bool walkable = index != Vector3Int.zero && Random.value > 0.15f;
            return new NodeData(index, walkable);
        }
    }
}
