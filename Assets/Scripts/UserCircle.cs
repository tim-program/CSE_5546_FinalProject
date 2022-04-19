using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCircle : MonoBehaviour
{
    public LineRenderer circleRender;
    // Start is called before the first frame update
    void Start()
    {
        DrawCircle(500, 450);
    }

    // To achieve GUI movement, we can essentially +1 or -1 on steps, based on stick input while selecting
    public void DrawCircle(int steps, float radius)
    {
        Debug.Log("Drawing Circle");
        circleRender.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float)currentStep / (steps - 1);

            float currentRadian = circumferenceProgress * 2 * Mathf.PI;

            float xScaled = Mathf.Cos(currentRadian);
            float zScaled = Mathf.Sin(currentRadian);

            float x = radius * xScaled;
            float y = 0;
            float z = radius * zScaled;

            Vector3 currentPosition = new Vector3(x, y, z);

            circleRender.SetPosition(currentStep, currentPosition);

        }
    }

}
