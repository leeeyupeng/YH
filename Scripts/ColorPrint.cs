/*********************************************************************************
 *Copyright(C) 2015 by LiYupeng
 *All rights reserved.
 *FileName:     ColorPrint.cs
 *Author:       LiYupeng
 *Version:      1.0
 *UnityVersion：5.4.0f3
 *Date:         2016-11-11
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

public class ColorPrint : MonoBehaviour {
    void Start()
    {
        Mesh m = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        Color[] colors = m.colors;
        foreach(Color c in colors)
        {
            Debug.Log(c.a);
        }
    }
}
