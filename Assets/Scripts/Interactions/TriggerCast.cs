using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCast : MonoBehaviour
{

    [SerializeField]
    private OVRPointerVisualizer ovrPtr;
    [SerializeField]
    private LineRenderer lineRend; 
    
    public bool rendLine;


    private readonly OVRInput.RawButton rTrigger = OVRInput.RawButton.RIndexTrigger, 
        lTrigger = OVRInput.RawButton.LIndexTrigger;

    // Start is called before the first frame update
    void Start()
    {
        lineRend.enabled = false;
        rendLine = false;
        DetectAndToggle();
    }

    //Called from WSI Image Input Callback Function 'Event Trigger'
    // On Drag Start, & On Drag End
    public void DetectAndToggle()
    {
        
        if (OVRInput.Get(rTrigger))
        {
            rendLine = true;
        } else if (OVRInput.Get(lTrigger))
        {
            rendLine = true;
        } else
        {
            rendLine = false;
        }

        lineRend.enabled = rendLine;
    }
    
}
