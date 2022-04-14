using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastScript : MonoBehaviour
{
    [SerializeField] GameObject Heatmap;
    QuadScript Quad;

    void Start()
    {
        Quad = Heatmap.GetComponent<QuadScript>();
        StartCoroutine(CheckHeadsetForward());
    }

    private void OnApplicationQuit()
    {
        StopCoroutine(CheckHeadsetForward());
    }

    private IEnumerator CheckHeadsetForward()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.collider.gameObject == Heatmap)
                {
                    Debug.Log("Hot spot quad hit [" + hit.textureCoord.x + ", " + hit.textureCoord.y + "]");
                    Quad.AddHitPoint(
                        //shader uses a range of -2.0f to 2.0f (IDK why but use this for now)
                        RemapRange(hit.textureCoord.x, 0.0f, 1.0f, -2.0f, 2.0f),
                        RemapRange(hit.textureCoord.y, 0.0f, 1.0f, -2.0f, 2.0f));
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
}
