using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardView : MonoBehaviour
{
    [SerializeField] private GridAnimationController animations;
    [SerializeField] private NodeFactory factory;
    [SerializeField] private BoardManager manager;

    [SerializeField] private NodeView startNode;

    private Dictionary<NodeData, NodeView> nodes = new Dictionary<NodeData, NodeView>();

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
        
        foreach (var entry in board)
        {
            NodeView node = entry.Key == Vector3Int.zero ? startNode : factory.CreateNode(entry.Key);
            nodes[entry.Value] = node;
        }

        foreach(var entry in nodes)
        {
            NodeView node = entry.Value;
            NodeData data = entry.Key;
            IEnumerable<NodeView> neighbours = data.Neighbours.Select(n => nodes[n]);
            node.Set(data, neighbours, OnNodeClicked);
            node.Pop();
        }
    }

    public void ClearBoard()
    {
        foreach (var entry in nodes)
        {
            if (entry.Value != startNode)
            {
                Destroy(entry.Value.gameObject);
            }
        }
        nodes.Clear();
    }

    private void OnNodeSelectionChanged(NodeData node, bool selected, PointerEventData pointerData)
    {
        if(nodes.TryGetValue(node, out NodeView view))
        {
            view.ToggleHeight(selected);
            NodeUtility.BroadSearch(2, view, v => v.Data.Neighbours.Select(n => nodes[n]), null, (n, i) => OnSelectSpread(n, i, selected));
        }
    }

    private void OnSelectSpread(NodeView node, int depth, bool selected)
    {
        if (selected)
        {
            node.SetPullHeight(depth);
        }
        else
        {
            node.ToggleHeight(false);
        }
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
