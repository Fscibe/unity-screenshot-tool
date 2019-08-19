using UnityEditor;
using UnityEngine;

public static class PackageBuilder
{
	[MenuItem("PackageBuilder/Export package")]
	public static void BuildPackage()
	{
		const string PACKAGE_NAME = "ScreenshotTool";

		var folders = new string[]{
			"Assets/" + PACKAGE_NAME
		};

		var guids = AssetDatabase.FindAssets("", folders);
		var assets = new string[guids.Length];
		for (int i = 0; i < guids.Length; ++i)
		{
			assets[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
		}

		string exportPath = Application.dataPath;
		var file = EditorUtility.SaveFilePanel("Export Package", exportPath, PACKAGE_NAME, "unitypackage");
		if (!string.IsNullOrEmpty(file))
		{
			AssetDatabase.ExportPackage(assets, file, ExportPackageOptions.Default);
		}
	}
}
