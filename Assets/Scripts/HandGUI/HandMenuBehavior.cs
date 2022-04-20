using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuBehavior : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] UnityEngine.UI.Image debugImage;
    [SerializeField] UnityEngine.UI.Button debugButton;

    [SerializeField] UnityEngine.UI.Text toggleText;
    [SerializeField] UnityEngine.UI.Toggle toggle;

    [SerializeField] UnityEngine.UI.Text debugText;

    private void Update()
    {
        if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.7f)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    public void Debug()
    {
        debugImage.color = new Color(Random.value, Random.value, Random.value);
    }

    public void ToggleChanged()
    {
    }

    public void Quit()
    {
        Application.Quit();
    }
   

}
