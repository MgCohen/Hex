using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoordinateDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coordinates;
    [SerializeField] private CanvasGroup group;

    private Vector3 startPos;
    private Vector3 moveVector = new Vector3(0, 100);

    private void Awake()
    {
        startPos = transform.position;
        transform.position = startPos + moveVector;
    }

    public void Hide()
    {
        transform.DOMove(startPos + moveVector, 0.5f);
        group.DOFade(0, 0.5f);
    }

    public void Show()
    {
        group.DOFade(1, 0.5f);
        transform.DOMove(startPos, 0.5f);
    }


    public void Set(Vector3Int position)
    {
        coordinates.text = position.ToString();
    }
}
