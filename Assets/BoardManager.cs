using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int size;

    private BoardNavigation navigation;
    private GridBuilder builder;
    private Board board;

    public event Action<Board> OnBoardChanged = delegate { };
    public event Action OnBoardCleared = delegate { };
    public event Action<List<NodeData>> OnPathChanged = delegate { };
    public event Action<NodeData, bool, PointerEventData> OnNodeSelectionChanged = delegate { };

    private List<NodeData> selectedNodes = new List<NodeData>();

    private void Awake()
    {
        builder = new GridBuilder();
        navigation = new BoardNavigation(this);
    }

    private void ToggleBoard()
    {
        if (board != null)
        {
            board.Clear();
            board = null;
            OnBoardCleared?.Invoke();
        }
        else
        {
            board = builder.CreateBoard(size);
            OnBoardChanged?.Invoke(board);
        }
    }

    public void OnNodeClicked(NodeData clickedNode, PointerEventData pointerData)
    {
        if (clickedNode.Index == Vector3Int.zero)
        {
            ToggleBoard();
            return;
        }

        ToggleNodeSelection(clickedNode, pointerData);
    }

    public void ToggleNodeSelection(NodeData node, PointerEventData pointerData)
    {
        if (selectedNodes.Contains(node))
        {
            selectedNodes.Remove(node);
            OnNodeSelectionChanged?.Invoke(node, false, pointerData);
        }
        else
        {
            selectedNodes.Add(node);
            OnNodeSelectionChanged?.Invoke(node, true, pointerData);
        }
    }

    public void MarkCells(List<NodeData> path)
    {

    }

}
