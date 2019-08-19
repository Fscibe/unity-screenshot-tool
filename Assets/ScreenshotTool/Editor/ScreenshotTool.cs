using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Take screenshots of the Game window, at selected resolution.
/// </summary>
public class ScreenshotToolWindow : EditorWindow
{
	private const string SAVE_KEY_FILENAME = "ScreenshotToolWindow_FileName";

	private string	_filename = "";
	private bool	_addResolutionSuffix = true;
	private string	_lastSavedFilename;
	private string	_folderName = "screenshots";
	private string	_outputDirectoryPath = "";

	[MenuItem("Tools/Screenshot")]
	static void Init()
	{
		ScreenshotToolWindow instance = GetWindow(typeof(ScreenshotToolWindow), false, "Screenshot") as ScreenshotToolWindow;
		instance._filename = EditorPrefs.GetString(SAVE_KEY_FILENAME, "screenshot");
	}

	private void CreateOutputDirectory()
	{
		if (!Directory.Exists(_outputDirectoryPath))
		{
			Directory.CreateDirectory(_outputDirectoryPath);
		}
	}

	private void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		{
			_outputDirectoryPath = Application.persistentDataPath + "/" + _folderName;

			// screenshot name
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField("Screenshot name", GUILayout.Width(128f));

				EditorGUI.BeginChangeCheck();
				_filename = GUILayout.TextField(_filename);
				if (EditorGUI.EndChangeCheck())
				{
					EditorPrefs.SetString(SAVE_KEY_FILENAME, _filename);
				}
			}
			EditorGUILayout.EndHorizontal();

			_addResolutionSuffix = GUILayout.Toggle(_addResolutionSuffix, "Add screen resolution to base name");

			// capture
			if (GUILayout.Button("Take screenshot!"))
			{
				// resolution
				string suffix = "";
				if (_addResolutionSuffix)
				{
					suffix = "_" + Camera.main.pixelWidth + "x" + Camera.main.pixelHeight;
				}

				// number
				int number = -1;
				string fullFileName;
				string fullFilePath;
				do
				{
					++number;
					fullFileName = _filename + suffix + "_" + number.ToString("000") + ".png";
					fullFilePath = _outputDirectoryPath + "/" + fullFileName;
					_lastSavedFilename = fullFileName;
				}
				while (File.Exists(fullFilePath) && number <= 999);

				// screenshot
				CreateOutputDirectory();
				ScreenCapture.CaptureScreenshot(fullFilePath);
			}

			// destination folder
			if (GUILayout.Button("Show destination folder"))
			{
				CreateOutputDirectory();
				EditorUtility.RevealInFinder(_outputDirectoryPath);
			}

			// capture feedback
			if (!string.IsNullOrEmpty(_lastSavedFilename))
			{
				EditorGUILayout.LabelField("Saved as '" + _lastSavedFilename + "'!");
			}
		}
		EditorGUILayout.EndVertical();
	}
}
