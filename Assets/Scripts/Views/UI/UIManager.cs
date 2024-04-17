using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hex.Controllers;
using Hex.Models;

namespace Hex.Views
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CoordinateDisplay leftDisplay;
        [SerializeField] private CoordinateDisplay rightDisplay;
        [SerializeField] private BoardManager manager;

        private void Start()
        {
            manager.OnBoardChanged += OnBoardChanged;
            manager.OnBoardCleared += OnBoardCleared;
            manager.OnNodeSelectionChanged += OnSelectionChanged;
        }

        private void OnBoardCleared()
        {
            leftDisplay.Hide();
            rightDisplay.Hide();
        }

        private void OnBoardChanged(Board board)
        {
            StartCoroutine(WaitAndShow());
        }

        private IEnumerator WaitAndShow()
        {
            yield return new WaitForSeconds(2.5f);
            leftDisplay.Show();
            rightDisplay.Show();
        }

        private void OnSelectionChanged(ICell start, ICell end, IList<ICell> path)
        {
            leftDisplay.Set(start != null ? start.Index : Vector3Int.zero);
            rightDisplay.Set(end != null ? end.Index : Vector3Int.zero);
        }
    }
}
