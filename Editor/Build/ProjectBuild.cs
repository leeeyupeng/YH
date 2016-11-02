using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
 
class ProjectBuild : Editor{
 
	//鍦ㄨ繖閲屾壘鍑轰綘褰撳墠宸ョ▼鎵鏈夌殑鍦烘櫙鏂囦欢锛屽亣璁句綘鍙?兂鎶婇儴鍒嗙殑scene鏂囦欢鎵撳寘 閭ｄ箞杩欓噷鍙?互鍐欎綘鐨勬潯浠跺垽鏂?鎬讳箣杩斿洖涓涓?瓧绗︿覆鏁扮粍銆佁
	static string[] GetBuildScenes()
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
	//shell鑴氭湰鐩存帴璋冪敤杩欎釜闈欐佹柟娉斕
	static void BuildForIPhone()
	{ 
        //鎵撳寘涔嬪墠鍏堣?缃?竴涓娺棰勫畾涔夋爣绛撅紝 鎴戝缓璁?ぇ瀹舵渶濂歼鍋氫竴浜氝 91 鍚屾?鎺н蹇?敤 PP鍔╂墜涓绫荤殑鏍囩?銆佭杩欐牱鍦ㄤ唬鐮佷腑鍙?互鐏垫椿鐨勫紑鍚?鎴栬呭叧闂?涓浜涗唬鐮併佁
        //鍥犱负 杩欓噷鎴戞槸鎵挎帴 涓婁竴绡囨枃绔狅紝 鎴戝氨浠?haresdk鍋氫緥瀛忂锛岃繖鏍锋柟渚垮ぇ瀹跺?涔熯锛嬏
		//PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iPhone, "USE_SHARE");
        //杩欓噷灏辨槸鏋勫缓xcode宸ョ▼鐨勬牳蹇冩柟娉曚簡锛嬤
        //鍙傛暟1 闇瑕佹墦鍖呯殑鎵鏈夊満鏅?
        //鍙傛暟2 闇瑕佹墦鍖呯殑鍚嶅瓙锛嬤杩欓噷鍙栧埌鐨勫氨鏄?shell浼犺繘鏉ョ殑瀛楃?涓边91
        //鍙傛暟3 鎵撳寘骞冲彴
		BuildPipeline.BuildPlayer(GetBuildScenes(), projectName, BuildTarget.iOS, BuildOptions.None);
	}
}