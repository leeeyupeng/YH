/*********************************************************************************
 *Copyright(C) 2015 by LiYupeng
 *All rights reserved.
 *FileName:     toolsArt.cs
 *Author:       LiYupeng
 *Version:      1.0
 *UnityVersion：5.4.0f3
 *Date:         2016-11-14
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

using UnityEditor;

public class toolsArt : Editor {
    [MenuItem("KOL/gen all Arts")]
    public static void GenAllArts()
    {

        ActorEditor.AutoGen();


    }
}
