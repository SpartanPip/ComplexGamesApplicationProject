using UnityEngine;
using UnityEngine.EventSystems;

public class HoverableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rectTransform;
    private Vector2 startingScale;

    private void Start()
    {
        startingScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IncreaseWidth();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DecreaseWidth();
    }

    private void IncreaseWidth()
    {
        rectTransform.localScale = startingScale * 1.1f;
    }

    public void DecreaseWidth()
    {
        rectTransform.localScale = startingScale;
    }
}
