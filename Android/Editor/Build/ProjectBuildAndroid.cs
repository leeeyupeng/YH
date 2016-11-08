/*********************************************************************************
 *Copyright(C) 2015 by LiYupeng
 *All rights reserved.
 *FileName:     ProjectBuildAndroid.cs
 *Author:       LiYupeng
 *Version:      1.0
 *UnityVersion：5.4.0f3
 *Date:         2016-11-04
 *Description:   
 *History:  
**********************************************************************************/
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

public class ProjectBuildAndroid : ProjectBuild
{
    public static void BuildForAndroid()
    {
        BuildPipeline.BuildPlayer(GetBuildScenes(), projectName, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("KOL/build/export androd project")]
    public static void BuildForEclipse()
    {
        BuildPipeline.BuildPlayer(GetBuildScenes(), projectName, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }
    //[MenuItem("KOL/build/export androd project test")]
    //public static void test()
    //{
    //    foreach (string arg in System.Environment.GetCommandLineArgs())
    //    {
    //        if (arg.StartsWith("project"))
    //        {
    //            return arg.Split("-"[0])[1];
    //        }
    //    }
    //}
}
