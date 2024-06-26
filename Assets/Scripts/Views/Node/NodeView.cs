using Hex.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hex.Views
{
    public class NodeView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public Vector3Int Index { get; private set; }
        public IEnumerable<NodeView> Neighbours { get; private set; }
        public ICell Data { get; private set; }
        public NodeAnimations Animations => animations;

        [SerializeField] protected NodeAnimations animations;

        private event Action<NodeView, PointerEventData> onClick;

        public virtual void Set(ICell data, IEnumerable<NodeView> neighbours, Action<NodeView, PointerEventData> onClick)
        {
            this.onClick = onClick;

            transform.position = new Vector3();

            //set values
            Data = data;
            Index = data.Index;
            Neighbours = neighbours;
        }

        public void SetHeight(float height)
        {
            animations.DoNodeHeight(height, 0.2f, DG.Tweening.Ease.Linear, true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            animations.DoClickPump(() =>
            {
                onClick?.Invoke(this, eventData);
            });
        }

        public virtual void Pop(int intensity)
        {
            animations.DoStartPop(Data.Walkable, intensity);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            animations.TogglePointerColor(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            animations.TogglePointerColor(true);
        }
    }
}