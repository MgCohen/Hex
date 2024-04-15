using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public class NodeVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float offHeight = 3f;
    [SerializeField] private float onHeight = 4f;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color normalColor;

    private Color startColor;

    Tween currentTween;

    private void Awake()
    {
        startColor = sprite.color;
        sprite.size = new Vector2(sprite.size.x, offHeight);
    }

    public void DoClickPump(Action action)
    {
        DoNodePunch(-0.3f, 0.15f, 0).OnComplete(action.Invoke);
    }

    public void DoStartPop(bool walkable)
    {
        float random = UnityEngine.Random.value / 2;
        
        if (!walkable)
        {
            DoSpriteColor(Color.grey, 0.2f, true).SetDelay(0.1f + random);
        }

        DoNodePunch(-0.3f, 0.3f, 0).SetDelay(random).OnComplete(() =>
        {
            if (!walkable)
            {
                DoNodeHeight(0.5f, 0.1f, Ease.Linear, true);
            }
        });

        transform.DOScale(0.95f, 0.2f).SetDelay(0.15f + random);
    }

    public void SetHeightUp()
    {
        DoNodeHeight(onHeight, 0.2f);
    }


    public void SetHeightDown()
    {
        DoNodeHeight(offHeight, 0.2f);
    }

    public Tween DoSpriteColor(Color color, float time = 0, bool setAsDefaultColor = false)
    {
        if (setAsDefaultColor)
        {
            startColor = color;
        }

        if (time == 0)
        {
            sprite.color = color;
            return null;
        }

        return sprite.DOColor(color, time);
    }

    public Tween DoNodeHeight(float height, float time, Ease ease = Ease.Linear, bool useRelativeHeight = false)
    {
        if (currentTween.IsActive())
        {
            currentTween.Kill();
        }

        if (useRelativeHeight)
        {
            height = ((onHeight - offHeight) * height) + offHeight;
        }

        float currentHeight = sprite.size.y;
        currentTween = DOTween.To(() => currentHeight, x => currentHeight = x, height, time).SetEase(ease).OnUpdate(() =>
        {
            sprite.size = new Vector2(sprite.size.x, currentHeight);
        });
        return currentTween;
    }

    private Tween DoNodePunch(float height, float time, int vibrato = 10, int elasticity = 1)
    {
        if (currentTween.IsActive())
        {
            currentTween.Kill(true);
        }

        Vector3 direction = new Vector3(0, height, 0);
        float currentHeight = sprite.size.y;

        currentTween = DOTween.Punch(() => sprite.size, x => sprite.size = x, direction, time, vibrato, elasticity);
        return currentTween;
    }

    public void TogglePointerColor(bool state)
    {
        sprite.DOKill(false);
        Color color = state ? hoverColor: normalColor;
        sprite.DOColor(startColor * color, 0.1f);
    }
}
