using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvancedButton : Selectable, IPointerClickHandler
{
    [Header("Click Events")]
    public UnityEvent OnLeftClick;
    public UnityEvent OnMiddleClick;
    public UnityEvent OnRightClick;

    private Coroutine resetRoutine;

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();

        Image imageComponent = GetComponent<Image>();
        if(imageComponent == null)
        {
            imageComponent = gameObject.AddComponent<Image>();
        }

        targetGraphic = imageComponent;
    }
#endif

    public void OnPointerClick(PointerEventData eventData)
    {
        DoStateTransition(SelectionState.Pressed, true);

        switch (eventData.button)
        {
            default: break;
            case PointerEventData.InputButton.Left:
                OnLeftClick?.Invoke();
                break;
            case PointerEventData.InputButton.Middle:
                OnMiddleClick?.Invoke();
                break;
            case PointerEventData.InputButton.Right:
                OnRightClick?.Invoke();
                break;
        }

        if(resetRoutine != null)
        {
            StopCoroutine(OnFinishSubmit());
        }

        resetRoutine = StartCoroutine(OnFinishSubmit());
    }

    IEnumerator OnFinishSubmit()
    {
        float fadeTime = colors.fadeDuration;
        float elapsedTime = 0f;

        while(elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }
}
