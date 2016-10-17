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

    [MenuItem("KOL/UI/Seperate Alpha For Material Select")]
    public static void SeperateAlpha_ForMaterialSelect()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets))
        {
            if (!(o is Material)) continue;
            //if (o.name.Contains("@")) continue;
            //if (!AssetDatabase.GetAssetPath(o).Contains(fbxPath)) continue;

            Material mat = (Material)o;
            Debug.Log("seperate : " + AssetDatabase.GetAssetPath(mat));
            SeperateAlpha(mat);
        }
    }

    [MenuItem("KOL/UI/Seperate Alpha For Texture Select")]
    public static void SeperateAlpha_ForTextureSelect()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets))
        {
            if (!(o is Texture2D)) continue;
            //if (o.name.Contains("@")) continue;
            //if (!AssetDatabase.GetAssetPath(o).Contains(fbxPath)) continue;

            Texture2D texture = (Texture2D)o;
            UITextureForETC1.SeperateRGBAandlphaChannel(AssetDatabase.GetAssetPath(texture));
        }
    }

    [MenuItem("KOL/UI/Seperate Alpha Only To ETC For Texture Select")]
    public static void SeperateAlphaToETC_ForTextureSelect()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets))
        {
            if (!(o is Texture2D)) continue;
            //if (o.name.Contains("@")) continue;
            //if (!AssetDatabase.GetAssetPath(o).Contains(fbxPath)) continue;

            Texture2D texture = (Texture2D)o;
            ToETC1(AssetDatabase.GetAssetPath(texture));
        }
    }
    public static void SeperateAlpha(Material mat)
    {
        mat.shader = Shader.Find(mat.shader.name.Replace(" Seperate", ""));
        string texturePath = AssetDatabase.GetAssetPath(mat.GetTexture("_MainTex"));
        texturePath = texturePath.Replace("_RGB","");
        mat.SetTexture("_MainTex", AssetDatabase.LoadAssetAtPath(texturePath,typeof(Texture2D)) as Texture2D);

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
            mat.SetTexture("_AlphaTex", alphaTexture);
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

    static void ToETC1(string path)
    {
        try
        {
            AssetDatabase.ImportAsset(path);
        }
        catch
        {
            Debug.LogError("Import Texture failed: " + path);
            return;
        }

        TextureImporter importer = null;
        try
        {
            importer = (TextureImporter)TextureImporter.GetAtPath(path);
        }
        catch
        {
            Debug.LogError("Load Texture failed: " + path);
            return;
        }
        if (importer == null)
        {
            return;
        }
        //importer.maxTextureSize = Mathf.Max(width, height);
        importer.anisoLevel = 0;
        importer.isReadable = false;  //increase memory cost if readable is true
        importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
        importer.textureType = TextureImporterType.Image;
        //importer.SetAllowsAlphaSplitting(true);
        if (path.Contains("/UI/"))
        {
            importer.textureType = TextureImporterType.GUI;
        }
        AssetDatabase.ImportAsset(path);
    }
}
