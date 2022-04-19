using System;
using System.Collections.Generic;
using System.Linq;
using OpenSlideNET;
using UnityEngine;
using Random = UnityEngine.Random;

public class WSIManager
{
    public OpenSlideImage image;
    public DeepZoomGenerator dzGen;

    public int maxZoom
    {
        get { return image.LevelCount - 1; }
    }

    public long viewX, viewY;

    // Heatmap Stuff
    public List<float> heatmapPoints;

    public int hitCount
    {
        get { return heatmapPoints.Count; }
    }

    public WSIManager(string fileName, int windowSizeX = -1, int windowSizeY = -1, int tileSize = 254,
        int overlap = 1)
    {
        Debug.Log(OpenSlideImage.LibraryVersion);
        Debug.Log($"Loading file {fileName}");
        image = OpenSlideImage.Open(fileName);
        viewX = windowSizeX > 0 ? windowSizeX : image.Height;
        viewY = windowSizeY > 0 ? windowSizeY : image.Width;
        dzGen = new DeepZoomGenerator(image, tileSize, overlap);

        // HeatMap Inits
        heatmapPoints = new List<float>();
    }

    public Texture2D GetTilesAtZoomAndPos(int zoomLevel, int centerX, int centerY)
    {
        zoomLevel = maxZoom - zoomLevel;
        // Debug.Log($"Image Dims: {(image.Height, image.Width)}");
        // Debug.Log($"ZoomAndPos Params: {(zoomLevel, centerX, centerY)}");
        long x = (long) Mathf.Clamp(centerX - viewX / 2, 0, image.Height);
        long y = (long) Mathf.Clamp(centerY - viewY / 2, 0, image.Width);
        // Debug.Log($"ReadRegion Params: {(zoomLevel, y, x, viewY, viewX)}");
        byte[] portion = image.ReadRegion(zoomLevel, y, x,
            viewY, viewX);
        Texture2D img = new Texture2D((int) viewX, (int) viewY, TextureFormat.ARGB32, false);
        img.LoadRawTextureData(portion);

        return img;
    }

    public int GetZeroCount()
    {
        return 0;
    }

    public void AddHitPoint(float x, float y)
    {
        heatmapPoints.Add(x);
        heatmapPoints.Add(y);
        heatmapPoints.Add(Random.Range(1f, 3f));
    }

    private float RemapRange(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
}
