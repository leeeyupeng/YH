/*********************************************************************************
 *Copyright(C) 2015 by LiYupeng
 *All rights reserved.
 *FileName:     MecanimEditor.cs
 *Author:       LiYupeng
 *Version:      1.0
 *UnityVersion：5.4.0f3
 *Date:         2016-10-27
 *Description:   
 *History:  
**********************************************************************************/
using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using System.IO;
using UnityEditor;
using UnityEditor.Animations;

public class MecanimEditor
{
    static string m_fbxPath = "Assets/Arts/Charactor";
    static string m_prefabPath = "Assets/Resources/Arts/Charactor";
    static string m_animatorPath = "Assets/Project/Arts/Charactor";

    [MenuItem("KOL/Actor/Mecanim/Gen Select")]
    public static void GenMecanimSelect()
    {
        string[] guids = Selection.assetGUIDs;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.IndexOf(".") != -1)
                continue;

            //if (!path.Contains(m_fbxPath)) continue;

            GenMecanim(path);
        }
    }

    public static void GenMecanim(string mecanimPath)
    {
        string[] guids = AssetDatabase.FindAssets("t:GameObject", new string[] { mecanimPath });
        string modelPath = null;
        foreach (string guid in guids)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(guid));
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (!path.Contains("@"))
            {
                modelPath = path;
            }
        }
        if (modelPath != null)
        {
            string prefabPath = modelPath.Replace(m_fbxPath, m_prefabPath);
            //string prefabPath = string.Format("{0}/{1}", m_prefabPath, Path.GetFileName(modelPath));
            prefabPath = prefabPath.Replace(".fbx", ".prefab");
            prefabPath = prefabPath.Replace(".FBX", ".prefab");
            string folder = prefabPath.Substring(0, prefabPath.LastIndexOf("/"));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            GameObject objModel = AssetDatabase.LoadAssetAtPath(modelPath, typeof(GameObject)) as GameObject;
            GameObject prefab = PrefabUtility.CreatePrefab(prefabPath, objModel);
            GameObject prefabInstantiate = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            //prefabInstantiate.layer = LayerMask.NameToLayer("Monster");
            Animator animator = prefabInstantiate.GetComponent<Animator>();
            animator.runtimeAnimatorController = GenController(mecanimPath);

            PrefabUtility.ReplacePrefab(prefabInstantiate, prefab, ReplacePrefabOptions.ConnectToPrefab);
            EditorUtility.SetDirty(prefab);

            GameObject.DestroyImmediate(prefabInstantiate);

        }
    }

    public static AnimatorController GenController(string fbxPath)
    {
        DirectoryInfo folder = new DirectoryInfo(fbxPath);
        string fbxPathBones = "";
        List<string> fbxPathClip = new List<string>();
        foreach (FileInfo file in folder.GetFiles())
        {
            if (file.Extension == ".fbx" || file.Extension == ".FBX")
            {
                if (file.Name.Contains("@"))
                {
                    fbxPathClip.Add(file.FullName);
                }
                else
                {
                    fbxPathBones = file.FullName;
                }
            }
        }
        fbxPathBones = fbxPathBones.Replace("\\", "/");
        fbxPathBones = fbxPathBones.Replace(Application.dataPath, "Assets");
        string animatorPath = fbxPathBones.Replace(m_fbxPath, m_animatorPath);
        //string animatorPath = string.Format("{0}/{1}", m_animatorPath, Path.GetFileName(fbxPathBones));
        animatorPath = string.Format("{0}\\{1}.controller", Path.GetDirectoryName(animatorPath), Path.GetFileNameWithoutExtension(animatorPath));
        animatorPath = animatorPath.Replace("\\", "/");
        animatorPath = animatorPath.Replace(Application.dataPath, "Assets");
        //animatorPath = "Assets/Mecanim/StateMachineTransitions.controller";
        if (!Directory.Exists(Path.GetDirectoryName(animatorPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(animatorPath));
        }
        //创建animationController文件，保存在Assets路径下
        AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(animatorPath);
        //得到它的Layer， 默认layer为base 你可以去拓展
        AnimatorControllerLayer layer = animatorController.layers[0];

        foreach (string clip in fbxPathClip)
        {
            AddStateTransition(clip, layer);
        }

        return animatorController;
    }

    private static void AddStateTransition(string path, AnimatorControllerLayer layer)
    {
        path = path.Replace("\\", "/");
        path = path.Replace(Application.dataPath, "Assets");

        AnimatorStateMachine sm = layer.stateMachine;
        //根据动画文件读取它的AnimationClip对象
        AnimationClip newClip = AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip)) as AnimationClip;
        //取出动画名子 添加到state里面
        AnimatorState state = sm.AddState(newClip.name);
        state.motion = newClip;
        //把state添加在layer里面
        //AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
        //把默认的时间条件删除
        //trans.RemoveCondition(0);
        //trans.hasExitTime = true;
        //trans.exitTime = 0f;
        if (path.Contains("Idle") || path.Contains("idle"))
        {
            sm.defaultState = state;
        }
    }
}
