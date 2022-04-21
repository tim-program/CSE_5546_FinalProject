using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenSlideNET;

public class QuadScript : MonoBehaviour
{
    public Material quadMaterial;
    MeshRenderer quadMeshRenderer;

    public float[] points;
    public float[][] pointss;
    int hitCount;
    int maxPoints = 256;
    int currentArr = 0;

    private void Start()
    {
        quadMeshRenderer = GetComponent<MeshRenderer>();
        quadMaterial = quadMeshRenderer.material;

        points = new float[maxPoints * 3]; //32 allowed points on heat map
        pointss = new float[3][];

        pointss[0] = new float[maxPoints * 3];
        pointss[1] = new float[maxPoints * 3];
        pointss[2] = new float[maxPoints * 3];
        //send x coord, y coord, intensity
    }

    private float ChangeValue(double value)
    {
        return Mathf.Min(1.0f, (float)(value / 255));
    }

    private void Update()
    {
        CheckMouseClickOnThis();
    }

    public void AddHitPoint(float x, float y)
    {
        //points[hitCount * 3] = x;
        //points[hitCount * 3 + 1] = y;
        //points[hitCount * 3 + 2] = Random.Range(1f, 3f);

        pointss[currentArr][hitCount * 3] = x;
        pointss[currentArr][hitCount * 3 + 1] = y;
        pointss[currentArr][hitCount * 3 + 2] = Random.Range(1f, 3f);

        hitCount++;
        hitCount %= maxPoints;

        //quadMaterial.SetFloatArray("_Hits", points);
        quadMaterial.SetFloatArray("_Hits", pointss[currentArr]);
        quadMaterial.SetInt("_HitCount", hitCount);
    }

    private void CheckMouseClickOnThis()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Hot spot quad hit [" + hit.textureCoord.x + ", " + hit.textureCoord.y + "]");
                    AddHitPoint(
                        //shader uses a range of -2.0f to 2.0f (IDK why but use this for now)
                        RemapRange(hit.textureCoord.x,0.0f,1.0f,-2.0f,2.0f), 
                        RemapRange(hit.textureCoord.y,0.0f,1.0f,-2.0f,2.0f));
                }
                else
                {
                    Debug.Log("What hit: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("Nothing was hit.");
            }
        }
    }

    private float RemapRange(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    public void SetCurrentPoints(int val)
    {
        currentArr = val;
    }
}
