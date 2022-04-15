using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WSI_Interactions : MonoBehaviour
{
    private Camera canvasCam;
    [SerializeField]
    private RectTransform window;
    private Vector2 initialTouchPos = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnDragStarted(BaseEventData data)
    {
        PointerEventData pointer = (PointerEventData)data;

        canvasCam = pointer.pressEventCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(window, pointer.pressPosition, canvasCam, out initialTouchPos);
    }

    public void OnDrag(BaseEventData data)
    {
        PointerEventData pointer = (PointerEventData)data;

        Vector2 touchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(window, pointer.position, canvasCam, out touchPos);
        window.anchoredPosition += touchPos - initialTouchPos;
    }

    public void OnEndDrag()
    {
        //EnsureWindowInBounds();
        print("Stopped Dragging");
    }

    /*
     * private void EnsureWindowInBounds()
    {
        Vector2 canvasSize = rectTransform.sizeDelta;
        Vector2 windowSize = windowTR.sizeDelta;

        if (windowSize.x < minWidth)
            windowSize.x = minWidth;
        if (windowSize.y < minHeight)
            windowSize.y = minHeight;

        if (windowSize.x > canvasSize.x)
            windowSize.x = canvasSize.x;
        if (windowSize.y > canvasSize.y)
            windowSize.y = canvasSize.y;

        Vector2 windowPos = windowTR.anchoredPosition;
        Vector2 canvasHalfSize = canvasSize * 0.5f;
        Vector2 windowHalfSize = windowSize * 0.5f;
        Vector2 windowBottomLeft = windowPos - windowHalfSize + canvasHalfSize;
        Vector2 windowTopRight = windowPos + windowHalfSize + canvasHalfSize;

        if (windowBottomLeft.x < 0f)
            windowPos.x -= windowBottomLeft.x;
        else if (windowTopRight.x > canvasSize.x)
            windowPos.x -= windowTopRight.x - canvasSize.x;

        if (windowBottomLeft.y < 0f)
            windowPos.y -= windowBottomLeft.y;
        else if (windowTopRight.y > canvasSize.y)
            windowPos.y -= windowTopRight.y - canvasSize.y;

        windowTR.anchoredPosition = windowPos;
        windowTR.sizeDelta = windowSize;
    }
    */
}
