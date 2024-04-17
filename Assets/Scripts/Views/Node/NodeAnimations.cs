using UnityEngine;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;


public class NodeAnimations : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float offHeight = 3f;
    [SerializeField] private float onHeight = 4f;

    [SerializeField] private Color hoverColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Gradient heightColor;

    private Color currentColor;
    private Tween currentTween;

    private void Awake()
    {
        currentColor = sprite.color;
        sprite.size = new Vector2(sprite.size.x, offHeight);
    }

    public void DoClickPump(Action action)
    {
        DoSizePunch(-0.3f, 0.15f, 0).OnComplete(action.Invoke);
    }

    public void DoStartPop(bool walkable, int intensity)
    {
        float random = UnityEngine.Random.Range(0f, 0.15f);
        float delay = random + (0.15f * intensity);
        float punch = 0.5f / intensity;

        if (!walkable)
        {
            DoSpriteColor(Color.grey, 0.2f, true).SetDelay(0.1f + delay);
        }

        transform.DOPunchPosition(new Vector3(0, punch, 0), 0.3f, 0).SetDelay(delay).OnComplete(() =>
        {
            if (!walkable)
            {
                DoNodeHeight(0.5f, 0.1f, Ease.OutBack, true, false);
            }
        });

        transform.DOScale(0.95f, 0.2f).SetDelay(0.15f + delay);
    }

    public Tween DoEndPop()
    {
        float random = Random.Range(0f, 0.4f);
        sprite.DOFade(0, 0.3f).SetDelay(0.1f + random);
        return transform.DOMoveY(-0.5f, 0.6f).SetRelative().SetEase(Ease.OutBack).SetDelay(random);
    }

    public void SetHeightUp()
    {
        DoNodeHeight(onHeight, 0.2f);
    }

    public Tween DoNodeHeight(float height, float time, Ease ease = Ease.Linear, bool useRelativeHeight = false, bool doColor = true)
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
        float delay = 0.1f * (1 - height);

        currentTween = DOTween.To(() => currentHeight, x => currentHeight = x, height, time).SetEase(ease).OnUpdate(() =>
        {
            sprite.size = new Vector2(sprite.size.x, currentHeight);
        }).SetDelay(delay);

        if (doColor)
        {
            float heightDiff = !useRelativeHeight ? height : ((height - offHeight) / (onHeight - offHeight));
            Color targetColor = heightColor.Evaluate(heightDiff);
            DoSpriteColor(targetColor, time, true).SetDelay(delay);
        }

        return currentTween;
    }

    private Tween DoSizePunch(float height, float time, int vibrato = 10, int elasticity = 1)
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
        Color pointerColor = state ? hoverColor : normalColor;
        Color color = pointerColor * currentColor;
        sprite.DOColor(color, 0.1f);
    }

    public Tween DoSpriteColor(Color color, float time = 0, bool setAsDefaultColor = false)
    {
        if (setAsDefaultColor)
        {
            currentColor = color;
        }

        sprite.DOKill(false);
        if (time == 0)
        {
            sprite.color = color;
            return null;
        }
        return sprite.DOColor(color, time);
    }
}
