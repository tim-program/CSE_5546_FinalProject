using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTo : MonoBehaviour
{

    [SerializeField] GameObject parent;
    [SerializeField] Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = parent.transform;
        gameObject.transform.localPosition = offset;
    }

}
