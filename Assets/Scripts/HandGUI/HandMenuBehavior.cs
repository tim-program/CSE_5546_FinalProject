using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuBehavior : MonoBehaviour
{

    [SerializeField] UnityEngine.UI.Image debugImage;
    [SerializeField] UnityEngine.UI.Button debugButton;

    [SerializeField] UnityEngine.UI.Text toggleText;
    [SerializeField] UnityEngine.UI.Toggle toggle;

    [SerializeField] UnityEngine.UI.Text debugText;

    private void Update()
    {
        debugText.name = string.Format("{0}",OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger));
    }

    public void Debug()
    {
        debugImage.color = new Color(Random.value, Random.value, Random.value);
    }

    public void ToggleChanged()
    {
        toggleText.text = toggle.isOn ? "On" : "Off";
    }

    public void Quit()
    {
        Application.Quit();
    }
   

}
