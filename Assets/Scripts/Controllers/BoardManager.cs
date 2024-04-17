using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Hex.Models;

namespace Hex.Controllers
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private int size;

        private IPathFinder navigation;
        private BoardBuilder builder;
        private Board board;

        public event Action<Board> OnBoardChanged = delegate { };
        public event Action OnBoardCleared = delegate { };
        public event Action<List<ICell>> OnPathChanged = delegate { };
        public event Action<ICell, ICell, IList<ICell>> OnNodeSelectionChanged = delegate { };

        private ICell selectedLeftNode;
        private ICell selectedRightNode;


        private void Awake()
        {
            builder = new BoardBuilder();
            navigation = new BoardNavigation();
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

        public void OnNodeClicked(ICell clickedNode, PointerEventData pointerData)
        {
            if (clickedNode.Index == Vector3Int.zero)
            {
                ToggleBoard();
                return;
            }

            ToggleNodeSelection(clickedNode, pointerData);
        }

        public void ToggleNodeSelection(ICell node, PointerEventData pointerData)
        {
            bool leftButton = pointerData.button == PointerEventData.InputButton.Left;
            if (leftButton)
            {
                if (node == selectedRightNode) selectedRightNode = null;
                selectedLeftNode = selectedLeftNode == node ? null : node;
            }
            else
            {
                if (node == selectedLeftNode) selectedLeftNode = null;
                selectedRightNode = selectedRightNode == node ? null : node;
            }

            IList<ICell> path = new List<ICell>();
            if (selectedLeftNode != null && selectedRightNode != null)
            {
                path = navigation.IFindPathOnMap(selectedLeftNode, selectedRightNode, default);
            }

            OnNodeSelectionChanged(selectedLeftNode, selectedRightNode, path);
        }
    }
}
