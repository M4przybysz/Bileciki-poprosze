using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    float topPositionLimit = 160f, bottomPositionLimit = -540f, rightPositionLimit = 960f, leftPositionLimit = -960f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 targetPosition = rectTransform.anchoredPosition + eventData.delta;
        rectTransform.anchoredPosition = new Vector2(
            Mathf.Clamp(targetPosition.x, leftPositionLimit + rectTransform.rect.width * rectTransform.localScale.x / 2, rightPositionLimit - rectTransform.rect.width * rectTransform.localScale.x / 2), 
            Mathf.Clamp(targetPosition.y, bottomPositionLimit + rectTransform.rect.height * rectTransform.localScale.y / 2, topPositionLimit - rectTransform.rect.height * rectTransform.localScale.y / 2));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
