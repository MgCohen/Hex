using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Hex.Models;
using Hex.Controllers;

namespace Hex.Views
{

    public class BoardView : MonoBehaviour
    {
        [SerializeField] private BoardAnimationController animations;
        [SerializeField] private NodeFactory factory;
        [SerializeField] private BoardManager manager;

        [SerializeField] private NodeView startNode;

        private Dictionary<ICell, NodeView> nodes = new Dictionary<ICell, NodeView>();
        private Dictionary<Vector3Int, NodeView> nodeMap = new Dictionary<Vector3Int, NodeView>();

        private void Awake()
        {
            NodeData startData = new NodeData(Vector3Int.zero, false);
            startNode.Set(startData, null, OnNodeClicked);

            manager.OnBoardChanged += SetBoard;
            manager.OnBoardCleared += ClearBoard;
            manager.OnNodeSelectionChanged += OnNodeSelectionChanged;
        }

        public void SetBoard(Board board)
        {
            nodes.Clear();
            nodeMap.Clear();

            foreach (var entry in board)
            {
                NodeView node = entry.Key == Vector3Int.zero ? startNode : factory.CreateNode(entry.Value);
                nodes[entry.Value] = node;
                nodeMap[entry.Key] = node;
            }

            foreach (var entry in nodes)
            {
                NodeView node = entry.Value;
                ICell data = entry.Key;
                Vector3Int[] neighbourIndexes = data.GetNodeNeighbourIndexes();
                IEnumerable<NodeView> neighbours = neighbourIndexes.Where(i => nodeMap.ContainsKey(i)).Select(i => nodeMap[i]);
                node.Set(data, neighbours, OnNodeClicked);
            }

            animations.AnimateBoardIn(startNode, nodeMap);
        }

        public void ClearBoard()
        {
            animations.AnimateBoardOut(startNode, nodeMap, () =>
            {
                nodes.Clear();
            });
        }

        private void OnNodeSelectionChanged(ICell start, ICell end, IList<ICell> path)
        {
            Dictionary<NodeView, float> heightMap = new Dictionary<NodeView, float>();
            if (path.Count > 0)
            {
                List<NodeView> viewPath = path.Select(n => nodes[n]).ToList();
                int pathSize = viewPath.Count;
                for (int i = 0; i < pathSize; i++)
                {
                    CalculateHeightMapWithPath(heightMap, viewPath[i], i, pathSize);
                }
            }
            else
            {
                if (start != null && nodes.TryGetValue(start, out NodeView view))
                {
                    NodeUtility.BroadSearch(2, view, v => v.Data.Neighbours.Select(n => nodes[n]), null, (n, i) => CalculateHeightMapWithoutPath(heightMap, n, i));
                }

                if (end != null && nodes.TryGetValue(end, out view))
                {
                    NodeUtility.BroadSearch(2, view, v => v.Data.Neighbours.Select(n => nodes[n]), null, (n, i) => CalculateHeightMapWithoutPath(heightMap, n, i));
                }
            }

            SetHeightMap(heightMap);
        }

        private void SetHeightMap(Dictionary<NodeView, float> heightMap)
        {
            foreach (var entry in nodes)
            {
                if (!entry.Key.Walkable)
                {
                    continue;
                }
                float height = heightMap.GetValueOrDefault(entry.Value);
                entry.Value.SetHeight(height);
            }
        }

        private void CalculateHeightMapWithPath(Dictionary<NodeView, float> heightMap, NodeView node, int depth, int pathSize)
        {
            pathSize = pathSize - 1;
            depth = Mathf.Max(depth, pathSize - depth);
            var newHeight = (float)depth / pathSize;
            if (heightMap.TryGetValue(node, out float height) && height > newHeight)
            {
                return;
            }
            heightMap[node] = newHeight;
        }

        private void CalculateHeightMapWithoutPath(Dictionary<NodeView, float> heightMap, NodeView node, int depth)
        {
            float newHeight = 1f / Mathf.Pow(2f, depth);
            if (heightMap.TryGetValue(node, out float height) && height > newHeight)
            {
                return;
            }
            heightMap[node] = newHeight;
        }

        private void OnNodeClicked(NodeView view, PointerEventData pointerData)
        {
            if (view != startNode && !view.Data.Walkable)
            {
                Camera.main.DOShakeRotation(0.3f, new Vector3(0, 0, 10), 30);
                return;
            }

            manager.OnNodeClicked(view.Data, pointerData);
        }
    }
}
