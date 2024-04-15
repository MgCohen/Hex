using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class NodeView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Vector3Int Index { get; private set; }
    public IEnumerable<NodeView> Neighbours { get; private set; }
    public NodeData Data { get; private set; }
    public NodeVisuals Visuals => visuals;

    [SerializeField] protected NodeVisuals visuals;

    private event Action<NodeView, PointerEventData> onClick;
    [SerializeField] private float height;

    public virtual void Set(NodeData data, IEnumerable<NodeView> neighbours, Action<NodeView, PointerEventData> onClick)
    {
        this.onClick = onClick;

        //set values
        Data = data;
        Index = data.Index;
        Neighbours = neighbours;
    }

    public void SetPullHeight(int depth)
    {
        //get all neighbours
        //get biggest size
        //get half
        if (depth == 0) return;
        var max = Neighbours.Max(n => n.height);
        this.height = max/2f;
        //var sizeChange = CalculateNodePull(depth);
        visuals.DoNodeHeight(height, 0.2f, DG.Tweening.Ease.Linear, true).SetDelay(0.1f * (depth - 1));
        visuals.DoSpriteColor(Color.yellow, 0.1f, true).SetDelay(0.1f * (depth - 1));
    }

    private float CalculateNodePull(int depth)
    {
        float pull = 1f / (2 * (depth - 1));
        return Mathf.Clamp01(pull);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        visuals.DoClickPump(() =>
        {
            onClick?.Invoke(this, eventData);
        });
    }

    public virtual void Pop()
    {
        visuals.DoStartPop(Data.Walkable);
    }

    public void ToggleHeight(bool state)
    {
        if (state)
        {
            height = 1;
            visuals.SetHeightUp();
        }
        else
        {
            height = 0;
            visuals.SetHeightDown();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        visuals.TogglePointerColor(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        visuals.TogglePointerColor(true);
    }
}
