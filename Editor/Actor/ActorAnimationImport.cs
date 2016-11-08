///*********************************************************************************
// *Copyright(C) 2015 by LiYupeng
// *All rights reserved.
// *FileName:     ActorAnimationImport.cs
// *Author:       LiYupeng
// *Version:      1.0
// *UnityVersion：5.4.0f3
// *Date:         2016-09-22
// *Description:   
// *History:  
//**********************************************************************************/
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//using UnityEditor;

//using System.IO;

//public class ActorAnimationImport
//{
//    public static string fbxPath = "Arts/Charactor/Human";
//    [MenuItem("KOL/Actor/Human/Animation Import")]
//    public static void ImportFolder(string folder)
//    {
//        DirectoryInfo di = new DirectoryInfo(folder);

//        foreach (FileInfo fi in di.GetFiles())
//        {
//            string path = fi.FullName;
//            path = path.Replace("\\", "/");
//            path = path.Replace(Application.dataPath, "Assets");
//            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

//            if (!(obj is GameObject)) continue;
//            if (!AssetDatabase.GetAssetPath(obj).Contains(fbxPath)) continue;

//            GameObject fbx = (GameObject)obj;
//            ModelImport(fbx);
//        }

//        foreach (DirectoryInfo cdi in di.GetDirectories())
//        {
//            ImportFolder(cdi.FullName);
//        }
//    }
//    static void ModelImport(GameObject fbx)
//    {
//        //ModelImporter modelImporter = assetImporter as ModelImporter;
//        string assetPath = AssetDatabase.GetAssetPath(fbx);
//        ModelImporter modelImporter = (ModelImporter)ModelImporter.GetAtPath(assetPath);

//        List<ModelImporterClipAnimation> animations = new List<ModelImporterClipAnimation>();
//        foreach (ModelImporterClipAnimation animation in modelImporter.defaultClipAnimations)
//        {
//            Debug.Log(assetPath);
//            if (!assetPath.Contains(AvatarEditor.fbxPath))
//                continue;

//            if (assetPath.Contains("idle"))
//            {
//                Debug.Log("idle");
//                animation.loop = true;
//                animation.loopTime = true;
//                animation.loopPose = true;
//            }
//            else if (assetPath.Contains("run"))
//            {
//                animation.loopTime = true;
//                animation.loopPose = true;
//            }
//            else if (assetPath.Contains("attack"))
//            {
//                Debug.Log("attack");
//                //animation.wrapMode = WrapMode.Loop;
//                animation.loop = true;
//                animation.loopTime = false;
//                animation.loopPose = false;
//                Debug.Log(animation.loopTime);
//            }

//            animations.Add(animation);
//        }

//        modelImporter.clipAnimations = animations.ToArray();
//    }
//}
