using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCircle : MonoBehaviour
{
    public LineRenderer circleRender;
    public Transform wsiOffset;
    public Transform player;

    private int currStepNum;
    private int totalSteps;
    private int currVertNum;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        radius = 450f;
        totalSteps = 500;
        currStepNum = 0;
        currVertNum = 0;
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

    public void GetCirclePoint(float xDelta, float yDelta)
    {
        Debug.Log("IMAGE MOVE:");
        int stepX = currStepNum;
        int stepY = currVertNum;
        if (xDelta >= 0.9001)
        {
            stepX++;
            if (stepX < 0)
            {
                stepX = totalSteps - 1;
            }
            else if (stepX > totalSteps)
            {
                stepX = 0;
            }
            Debug.Log("Moved Right");

        }
        else if (xDelta <= -0.9001)
        {
            stepX--;
            if (stepX < 0)
            {
                stepX = totalSteps - 1;
            }
            else if (stepX > totalSteps)
            {
                stepX = 0;
            }
            Debug.Log("Moved Left");
        }

        if (yDelta >= 0.9001)
        {
            //move up
            stepY++;
            if (stepY < -5)
            {
                stepY = -5;
            }
            else if (stepY > 5)
            {
                stepY = 5;
            }

        }
        else if (yDelta <= -0.9001)
        {
            // move down
            stepY--;
            if (stepY < -5)
            {
                stepY = -5;
            }
            else if (stepY > 5)
            {
                stepY = 5;
            }

        }
        Vector3 newPos = CirclePosition(stepX, stepY);
        currStepNum = stepX;
        currVertNum = stepY;

        wsiOffset.position = newPos;
        wsiOffset.LookAt(player);
        wsiOffset.Rotate(Vector3.up, -180f);
    }

    public Vector3 CirclePosition(int newStepSide, int newStepVert)
    {
        float newYVal = newStepVert * 10.0f;
        float circumferenceProgress = (float)newStepSide / (totalSteps - 1);

        float currentRadian = circumferenceProgress * 2 * Mathf.PI;

        float xScaled = Mathf.Cos(currentRadian);
        float zScaled = Mathf.Sin(currentRadian);

        float x = radius * zScaled;
        float y = newYVal;
        float z = radius * xScaled;


        return new Vector3(x, y, z);
    }

}
