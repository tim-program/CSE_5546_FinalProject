using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandMenuBehavior : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] UnityEngine.UI.Image debugImage;
    [SerializeField] UnityEngine.UI.Button debugButton;

    [SerializeField] UnityEngine.UI.Text toggleText;
    [SerializeField] UnityEngine.UI.Toggle toggle;

    [SerializeField] UnityEngine.UI.Text debugText;

    [SerializeField] GameObject quadObject;
    private MeshRenderer quadObjectMR;
    [SerializeField] Material[] materials;

    private void Start()
    {
        if (quadObject != null)
        {
            quadObjectMR = quadObject.GetComponent<MeshRenderer>();
            quadObjectMR.enabled = false;
        }
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.7f)
        {
            canvas.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    public void ChangeCaseNumber(int caseNum)
    {
        UnityEngine.Debug.Log($"Choosing case: {caseNum}");
        quadObjectMR.material = materials[caseNum];
    }

    public void ToggleChanged(Toggle change)
    {
        UnityEngine.Debug.Log(change.isOn);

        if (quadObject != null)
        {
            quadObject.GetComponent<MeshRenderer>().enabled = change.isOn;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
