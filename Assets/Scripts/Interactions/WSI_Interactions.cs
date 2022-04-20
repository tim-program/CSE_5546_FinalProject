using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WSI_Interactions : MonoBehaviour
{
    // Access these from anywhere! Be careful, though
    //public static GameObject activeLeftDevice;
    //public static GameObject activeRightDevice;

    public OVRInputModule ovrInput;
    public Image wsiImage;
    public WSIManager manager;
    public Transform wsiOffset;
    public UserCircle menuLock;

    private Camera canvasCam;
    [SerializeField] private RectTransform window;
    private Vector2 initialTouchPos = Vector2.zero;

    private int originX, originY, zoomLevel;

    // this is the number of frames we will run things like input checks, etc.
    private int counter = 0;
    private static int FRAME_LIM = 5;

    // Start is called before the first frame update
    void Start()
    {
        ReinitInteractions();
        // LoadWSI(Application.persistentDataPath + "/Data/Case19.tiff");
        ///LoadWSI("Assets/Scripts/WSIDemo/Melanoma_30_1.tiff");
        LoadWSI("./Case19.tif");
        //InputTypeCheck();
    }

    public void LoadWSI(string fileName)
    {
        manager = new WSIManager(fileName, windowSizeX: (int) window.rect.width, windowSizeY: (int) window.rect.height);
        originX = (int) (manager.image.Height / 2);
        originY = (int) (manager.image.Width / 2);
        Texture2D imTex = manager.GetTilesAtZoomAndPos(0, originX, originY);
        ApplyInteractedTexture(imTex);
    }

    public void OnDragStarted(BaseEventData data)
    {
        PointerEventData pointer = (PointerEventData) data;

        canvasCam = pointer.pressEventCamera;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(window, pointer.pressPosition, canvasCam,
            out initialTouchPos);
    }

    public void OnDrag(BaseEventData data)
    {
        PointerEventData pointer = (PointerEventData) data;

        Vector2 touchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(window, pointer.position, canvasCam, out touchPos);
        Vector2 newPos = touchPos - initialTouchPos;
        originX = (int) newPos.x;
        originY = (int) newPos.y;
        Texture2D imTex = manager.GetTilesAtZoomAndPos(zoomLevel, originX, originY);
        ApplyInteractedTexture(imTex);
    }

    //will move wsiOffset +1 or -1 step from UserCircle
    public void OnMoveCanvas(BaseEventData data)
    {
    }

    public void OnScaleCanvas(BaseEventData data)
    {
    }

    public void OnZoomIn(BaseEventData data)
    {
    }

    public void OnZoomOut(BaseEventData data)
    {
    }

    public void OnEndDrag()
    {
        //EnsureWindowInBounds();
        print("Stopped Dragging");
    }

    private void ApplyInteractedTexture(Texture2D imTex)
    {
        float ratio = ((float) imTex.height) / imTex.width;
        float x = 10;
        float y = x * ratio;
        //window.localScale = new Vector3(x, 1, y);
        imTex.Apply();
        //wsiImage.material.mainTexture = imTex;
        Rect rect = new Rect(0f, 0f, imTex.width, imTex.height);

        Sprite s = Sprite.Create(imTex, rect, new Vector2(.5f, .5f));
        wsiImage.sprite = s;
    }

    public void Update()
    {
        OVRInput.Update();
        DelayFunctions();
    }

    private void DelayFunctions()
    {
        if (counter % FRAME_LIM == 0)
        {
            /* Every function called in this logic
             * will have a 5+ (check value) frame delay.
             * This is useful for tons of reasons.
             */

            //InputTypeCheck();


            counter = 0;
        }

        counter++;
    }

    // Native Functions called w/ delay:

    private void InputTypeCheck()
    {
        //can switch inputs here but its not necessary
    }

    private void ReinitInteractions()
    {
        originX = 0;
        originY = 0;
        zoomLevel = 0;
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
