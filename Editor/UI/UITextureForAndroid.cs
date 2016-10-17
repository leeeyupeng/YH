/*********************************************************************************
 *Copyright(C) 2015 by #AUTHOR#
 *All rights reserved.
 *FileName:     #SCRIPTFULLNAME#
 *Author:       #AUTHOR#
 *Version:      #VERSION#
 *UnityVersion：#UNITYVERSION#
 *Date:         #DATE#
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

using UnityEditor;

public class UITextureForAndroid
{

    [MenuItem("KOL/UI/Texture Seperate Alpha Select")]
    public static void SeperateAlpha_Select()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets))
        {
            if (!(o is Material)) continue;
            //if (o.name.Contains("@")) continue;
            //if (!AssetDatabase.GetAssetPath(o).Contains(fbxPath)) continue;

            Material mat = (Material)o;
            SeperateAlpha(mat);
        }
    }
    public static void SeperateAlpha(Material mat)
    {
        Texture2D texture = mat.mainTexture as Texture2D;
        string texPath = AssetDatabase.GetAssetPath(texture);
        UITextureForETC1.SeperateRGBAandlphaChannel(texPath);

        string texRGBPath = UITextureForETC1.GetRGBTexPath(texPath);
        string texAlphaPath = UITextureForETC1.GetAlphaTexPath(texPath);
        string shaderName = mat.shader.name;
        if(shaderName == "Unlit/Transparent Colored")
        {
            mat.shader = Shader.Find("Unlit/Transparent Colored Seperate");

            Texture2D rgbTexture = AssetDatabase.LoadAssetAtPath(texRGBPath, typeof(Texture2D)) as Texture2D;
            mat.SetTexture("_MainTex", rgbTexture);
            Texture2D alphaTexture = AssetDatabase.LoadAssetAtPath(texAlphaPath, typeof(Texture2D)) as Texture2D;
            mat.SetTexture("_AlpahTex", alphaTexture);
            EditorUtility.SetDirty(mat);
        }
        else if (shaderName == "Unlit/Transparent")
        {
            mat.shader = Shader.Find("Unlit/Transparent Colored Seperate");

            Texture2D rgbTexture = AssetDatabase.LoadAssetAtPath(texRGBPath, typeof(Texture2D)) as Texture2D;
            mat.SetTexture("_MainTex", rgbTexture);
            Texture2D alphaTexture = AssetDatabase.LoadAssetAtPath(texAlphaPath, typeof(Texture2D)) as Texture2D;
            mat.SetTexture("_AlpahTex", alphaTexture);
            EditorUtility.SetDirty(mat);
        }


        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
