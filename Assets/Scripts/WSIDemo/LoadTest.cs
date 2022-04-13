using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenSlideNET;

public class LoadTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //OpenSlide Test Code
        Debug.Log("OpenSlideImage Version: " + OpenSlideImage.LibraryVersion);

        //Reads from root folder not Assets folder
        /*string format = OpenSlideImage.DetectFormat("Case12.tif");
        Debug.Log(
            format == null ?
            "File format not supported." :
            $"File format ({format}) supported.");*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
