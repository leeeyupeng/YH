using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
 
public class ProjectBuild : Editor{
 
	//鍦ㄨ繖閲屾壘鍑轰綘褰撳墠宸ョ▼鎵鏈夌殑鍦烘櫙鏂囦欢锛屽亣璁句綘鍙?兂鎶婇儴鍒嗙殑scene鏂囦欢鎵撳寘 閭ｄ箞杩欓噷鍙?互鍐欎綘鐨勬潯浠跺垽鏂?鎬讳箣杩斿洖涓涓?瓧绗︿覆鏁扮粍銆佁
	public static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();
 
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if(e==null)
				continue;
			if(e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}
 
    //寰楀埌椤圭洰鐨勫悕绉?
	public static string projectName
	{
		get
		{ 
			//鍦ㄨ繖閲屽垎鏋恠hell浼犲叆鐨勫弬鏁帮紝 杩樿?寰椾笂闈㈡垜浠??鐨勫摢涓┻project-$1 杩欎釜鍙傛暟鍚楋紵
			//杩欓噷閬嶅巻鎵鏈夊弬鏁帮紝鎵惧埌 project寮澶寸殑鍙傛暟锛嬤鐒跺悗鎶夓绗﹀彿 鍚庨潰鐨勫瓧绗︿覆杩斿洖锛嬏
		    //杩欎釜瀛楃?涓插氨鏄?91 浜嗐傘佁
			foreach(string arg in System.Environment.GetCommandLineArgs()) {
				if(arg.StartsWith("project"))
				{
					return arg.Split("-"[0])[1];
				}
			}
			return "Debug";
		}
	}

}