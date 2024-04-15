using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GridAnimationController : MonoBehaviour
{
    public void AnimateBoardIn(List<NodeView> views, Action onComplete = null)
    {
        StartCoroutine(BoardCreationSequence(views, onComplete));
    }

    public void AnimateBoardOut(List<NodeView> views, Action onComplete = null)
    {
        StartCoroutine(DestroyBoardSequence(views, onComplete));
    }

    private IEnumerator BoardCreationSequence(List<NodeView> views, Action onComplete)
    {
        yield return new WaitForSeconds(0.5f);
        PopBoard(views);
    }

    private IEnumerator DestroyBoardSequence(List<NodeView> views, Action onComplete)
    {
        var size = Camera.main.orthographicSize;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(Camera.main.DOOrthoSize(size - 2, 0.2f));
        sequence.Append(Camera.main.DOOrthoSize(size, 0.2f));
        yield return sequence.WaitForCompletion();
        onComplete?.Invoke();
    }

    private void PopBoard(List<NodeView> views)
    {
        foreach (var node in views)
        {
            node.Pop();
        }
    }
}
