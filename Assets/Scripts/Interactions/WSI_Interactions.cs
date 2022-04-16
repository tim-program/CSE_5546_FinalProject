using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WSI_Interactions : MonoBehaviour
{
    // Access these from anywhere! Be careful, though
    public static GameObject activeLeftDevice;
    public static GameObject activeRightDevice;

    public GameObject handTrackingLeft;
    public GameObject handTrackingRight;
    public GameObject controllerLeft;
    public GameObject controllerRight;
    public OVRInputModule ovrInput;

    private Camera canvasCam;
    [SerializeField]
    private RectTransform window;
    private Vector2 initialTouchPos = Vector2.zero;

    // this is the number of frames we will run things like input checks, etc.
    private int counter = 0;
    private static int FRAME_LIM = 5;

    // Start is called before the first frame update
    void Start()
    {
        InputTypeCheck();
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

    public void Update()
    {
        OVRInput.Update();
        DelayFunctions();
    }

    private void DelayFunctions()
    {
        if(counter % FRAME_LIM <= 0)
        {
            /* Every function called in this logic
             * will have a 5+ (check value) frame delay.
             * This is useful for tons of reasons.
             */

            InputTypeCheck();
            
            
            counter = 0;
        }
        counter++;
    }

    // Native Functions called w/ delay:

    private void InputTypeCheck()
    {
        // Hand Tracking:
        if(OVRPlugin.GetHandTrackingEnabled() == true)
        {
            handTrackingLeft.SetActive(true);
            handTrackingRight.SetActive(true);
            controllerLeft.SetActive(false);
            controllerRight.SetActive(false);
            // Assign active devices
            activeLeftDevice = handTrackingLeft;
            activeRightDevice = handTrackingRight;
        } else
        {
            handTrackingLeft.SetActive(false);
            handTrackingRight.SetActive(false);
            controllerLeft.SetActive(true);
            controllerRight.SetActive(true);
            activeLeftDevice = controllerLeft;
            activeRightDevice = controllerRight;
        }

        if(OVRInput.GetActiveController() == OVRInput.Controller.LTouch)
        {
            //Left controller active
        }

        if (OVRInput.GetActiveController() == OVRInput.Controller.RTouch)
        {
            //Right controller active
        }
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
