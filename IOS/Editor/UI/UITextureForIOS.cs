using UnityEngine;
using System.Collections;

using UnityEditor;

public class UITextureForIOS : MonoBehaviour {

	[MenuItem("KOL/UI/IOS/Compress")]
	public static void IOSCompress_ForMaterialSelect()
	{
		foreach (Object o in Selection.GetFiltered(typeof(Material), SelectionMode.DeepAssets))
		{
			if (!(o is Material)) continue;
			//if (o.name.Contains("@")) continue;
			//if (!AssetDatabase.GetAssetPath(o).Contains(fbxPath)) continue;

			Material mat = (Material)o;
			Debug.Log("seperate : " + AssetDatabase.GetAssetPath(mat));
			IOSCompress(mat);
		}
	}

	public static void IOSCompress(Material mat)
	{
		Texture2D texture = mat.mainTexture as Texture2D;
		CompressTexture (AssetDatabase.GetAssetPath(texture),texture.width,texture.height);
	}


	static void CompressTexture(string path, int width, int height)
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
		importer.maxTextureSize = Mathf.Max(width, height);
		importer.anisoLevel = 0;
		importer.isReadable = false;  //increase memory cost if readable is true

		if (width == height && false) {
			importer.textureFormat = TextureImporterFormat.AutomaticCompressed;
		} else {
			importer.textureFormat = TextureImporterFormat.Automatic16bit;
		}

		importer.textureType = TextureImporterType.Image;
		//importer.SetAllowsAlphaSplitting(true);
		if (path.Contains("/UI/"))
		{
			importer.textureType = TextureImporterType.GUI;
		}
		AssetDatabase.ImportAsset(path);
	}
}

