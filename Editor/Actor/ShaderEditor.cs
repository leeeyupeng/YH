/*********************************************************************************
 *Copyright(C) 2015 by LiYupeng
 *All rights reserved.
 *FileName:     ShaderEditor.cs
 *Author:       LiYupeng
 *Version:      1.0
 *UnityVersion：5.4.0f3
 *Date:         2016-10-27
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

using UnityEditor;
using System.IO;

public class ShaderEditor : MonoBehaviour
{
    public static string fbxPath = "Arts";

    [MenuItem("KOL/Actor/ShaderPC2Mobile")]
    public static void Shader2All()
    {
        Shader2Folder("Assets/" + fbxPath);
    }
    public static void Shader2Folder(string folder)
    {
        DirectoryInfo di = new DirectoryInfo(folder);

        foreach (FileInfo fi in di.GetFiles())
        {
            string path = fi.FullName;
            path = path.Replace("\\", "/");
            path = path.Replace(Application.dataPath, "Assets");
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Material));

            if (!(obj is Material)) continue;
            if (!AssetDatabase.GetAssetPath(obj).Contains(fbxPath)) continue;

            Material mat = (Material)obj;
            Shader2(mat);
        }

        foreach (DirectoryInfo cdi in di.GetDirectories())
        {
            Shader2Folder(cdi.FullName);
        }
    }

    public static void Shader2(Material mat)
    {
        if(mat.shader.name == "KOL/Charactor Scale Normal")
        {
            mat.shader = Shader.Find("KOL/Charactor Scale Normal Mobile");
        }
    }
}
