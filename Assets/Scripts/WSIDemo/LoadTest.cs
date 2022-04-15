using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenSlideNET;
using UnityEngine.UI;

public class LoadTest : MonoBehaviour
{
    public WSIManager manager;
    public int level = 0;
    public MeshRenderer mr;
    private static readonly int MainTexture = Shader.PropertyToID("_MainTexture");

    private int centerX, centerY;

    private Vector3 initMousePos;

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        //OpenSlide Test Code
        Debug.Log("OpenSlideImage Version: " + OpenSlideImage.LibraryVersion);

        // Please change this to something relevant to you
        string fileName = "Assets/Scripts/WSIDemo/Melanoma_30_1.tiff";

        manager = new WSIManager(fileName, windowSizeX: 1000, windowSizeY: 1000);
        centerX = (int) (manager.image.Height / 2);
        centerY = (int) (manager.image.Width / 2);
        Texture2D imTex = manager.GetTilesAtZoomAndPos(0, centerX, centerY);
        imTex.Apply();

        float ratio = ((float) imTex.height) / imTex.width;
        float x = 10;
        float y = x * ratio;
        transform.localScale = new Vector3(x, 1, y);
        mr.material.mainTexture = imTex;
    }

    private void OnMouseDown()
    {
        initMousePos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        // Debug.Log("Initial Position " + initMousePos);
        if (!initMousePos.Equals(Vector3.negativeInfinity))
        {
            Vector3 curMousePos = Input.mousePosition;
            // Debug.Log("Mouse Position" + curMousePos);
            Vector3 delta = curMousePos - initMousePos;
            centerX -= (int) delta.y;
            centerY -= (int) delta.x;
        }
    }

    private void OnMouseUp()
    {
        initMousePos = Vector3.negativeInfinity;
    }

    public void ChangeZoom(Slider zoomslider)
    {
        this.level = (int) zoomslider.value;
        zoomslider.maxValue = manager.maxZoom;
        zoomslider.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Center: {(centerX, centerY)}");
        Texture2D imTex =
            manager.GetTilesAtZoomAndPos(this.level, centerX, centerY);
        imTex.Apply();
        mr.material.mainTexture = imTex;
    }
}
