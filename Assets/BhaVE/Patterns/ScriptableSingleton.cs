using System;
using System.IO;
using UnityEditorInternal;
using UnityEngine;
namespace BhaVE.Patterns
{
	//https://forum.unity.com/threads/missing-documentation-for-scriptable-singleton.292754/
	// Crispy->Thomas: We'll use this to create a manager to handle the Editor configuration of the new Camera Capture Kit Manager
	// so that we can better handle editor singleton classes that require persistence, like Native Android Log Manager ( adb ) and iOS log Manager.
	// TODO: (Thomas) when a Native Photo is grapped from Android Camera we need to be able to capture the log as well as inspecting the camera
	// properties. When the project is reloaded we dont want the parametres to be reloaded.
	public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		static T instance;
		public static T singleton
		{
			get
			{
				if (ScriptableSingleton<T>.instance == null)
					CreateAndLoad();

				return instance;
			}
		}

		protected ScriptableSingleton()
		{
			if (ScriptableSingleton<T>.singleton != null)
				Debug.Log("Scriptable singleton already exists. Did you query the singleton in a constructor?");
			else
				instance = this as T;
		}

		static void CreateAndLoad()
		{
			string filePath = ScriptableSingleton<T>.GetFilePath();
			if (!string.IsNullOrEmpty(filePath))
			{
				InternalEditorUtility.LoadSerializedFileAndForget(filePath);
			}
			if (instance == null)
			{
				T t = CreateInstance<T>();
				t.hideFlags = HideFlags.HideAndDontSave;
			}
		}

		protected virtual void Save(bool saveAsText)
		{
			if (instance == null)
			{
				Debug.Log("Cannot save ScriptableSingleton: no instance!");
				return;
			}
			string filePath = GetFilePath();
			if (!string.IsNullOrEmpty(filePath))
			{
				string directoryName = Path.GetDirectoryName(filePath);
				if (!Directory.Exists(directoryName))
					Directory.CreateDirectory(directoryName);
				InternalEditorUtility.SaveToSerializedFileAndForget(new T[] { instance }, filePath, saveAsText);
			}
		}

		static string GetFilePath()
		{
			Type typeFromHandle = typeof(T);
			object[] customAttributes = typeFromHandle.GetCustomAttributes(true);
			object[] array = customAttributes;
			for (int i = 0; i < array.Length; i++)
			{
				object obj = array[i];
				if (obj is FilePathAttribute)
				{
					FilePathAttribute filePathAttribute = obj as FilePathAttribute;
					return filePathAttribute.filepath;
				}
			}
			return null;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class FilePathAttribute : Attribute
	{
		public enum Location
		{
			PreferencesFolder,
			ProjectFolder
		}
		public string filepath { get; set; }
		public FilePathAttribute(string relativePath, FilePathAttribute.Location location)
		{
			if (string.IsNullOrEmpty(relativePath))
			{
				Debug.LogError("Invalid relative path! (it's null or empty)");
				return;
			}
			if (relativePath[0] == '/')
				relativePath = relativePath.Substring(1);
			if (location == FilePathAttribute.Location.PreferencesFolder)
				filepath = InternalEditorUtility.unityPreferencesFolder + "/" + relativePath;
			else
				filepath = relativePath;
		}
	}
}