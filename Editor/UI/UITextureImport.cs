/*********************************************************************************
 *Copyright(C) 2015 by LiYupeng
 *All rights reserved.
 *FileName:     UITextureImport.cs
 *Author:       LiYupeng
 *Version:      1.0
 *UnityVersion：5.4.0f3
 *Date:         2016-09-22
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

using UnityEditor;

public class UITextureImport : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        //if (assetPath.Contains("UI") || assetPath.Contains("ui"))
        //{
        //    TextureImporter textureImporter = (TextureImporter)assetImporter;
        //    textureImporter.textureType = TextureImporterType.Sprite;
        //    textureImporter.mipmapEnabled = false;
        //    textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
        //    textureImporter.isReadable = true;
        //}

        if (assetPath.Contains("UI") || assetPath.Contains("ui"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;

            //SetTextureSprite(assetImporter,null);
        }
    }
    void OnPostprocessTexture(Texture2D texture)
    {
        //if (assetPath.Contains("UI") || assetPath.Contains("ui"))
        //{
        //}
    }

    private static void SetTextureSprite(AssetImporter ai, string tag = null)
    {
        TextureImporter importer = ai as TextureImporter;
        if (importer == null) return;
        importer.textureType = TextureImporterType.Sprite;
        importer.mipmapEnabled = false;
        importer.isReadable = false;
        if (tag != null)
        {
            tag = tag.ToLower();
            importer.spritePackingTag = tag;
        }

#if UNITY_ANDROID
        int maxSize = 1024;
        TextureImporterFormat format = TextureImporterFormat.AutomaticCompressed;
        int quality = 50;
        importer.GetPlatformTextureSettings("Android", out maxSize, out format, out quality);

        // 压缩的格式，android下修改为分离alpha通道的etc1  
        if (format == TextureImporterFormat.AutomaticCompressed)
        {
            importer.SetPlatformTextureSettings("Android", maxSize, TextureImporterFormat.ETC_RGB4, quality, true);
            importer.SetAllowsAlphaSplitting(true);
        }
#else
    // iPhone  
#endif
    }
}
