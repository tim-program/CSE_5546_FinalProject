using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandMenuBehavior : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] Image debugImage;
    [SerializeField] Button debugButton;

    [SerializeField] Text toggleText;
    [SerializeField] Toggle toggle;

    [SerializeField] Text debugText;

    [SerializeField] GameObject quadObject;
    [SerializeField] private Image parentImage;
    private MeshRenderer _quadObjectMr;
    [SerializeField] Material[] materials;

    private void Start()
    {
        if (quadObject != null)
        {
            _quadObjectMr = quadObject.GetComponent<MeshRenderer>();
            _quadObjectMr.enabled = false;
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
        Debug.Log($"Choosing case: {caseNum}");
        _quadObjectMr.material = materials[caseNum];
        quadObject.GetComponent<QuadScript>().quadMaterial = _quadObjectMr.material;
        quadObject.GetComponent<QuadScript>().SetCurrentPoints(caseNum);
        parentImage.material = materials[caseNum];
    }

    public void ToggleChanged(Toggle change)
    {
        Debug.Log(change.isOn);

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
