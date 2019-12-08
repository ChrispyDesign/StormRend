/* 
 *  Copyright (C) 2019 Totally Not Birds. All Rights Reserved.
 *  NOTICE: The intellectual and technical concepts contained herein are proprietary
 *  to Totally Not Birds and are protected by trade secret or copyright law.
 *  Dissemination of this information or proproduction of this material is strictly
 *  forbidden unless prior written permission is obtained forom Totally Not Birds.
 *  Written by Tony Le <letony@icloud.com> 2019
 */

using UnityEditor;

namespace StormRend.Editors
{
	public abstract class SmartEditor : Editor
	{
		public virtual string[] propertiesToExclude => new string[0];
		public virtual void OnPreInspector() { }
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			OnPreInspector();
			DrawPropertiesExcluding(serializedObject, propertiesToExclude);
			OnPostInspector();
			serializedObject.ApplyModifiedProperties();
		}
		public virtual void OnPostInspector() { }
	}
}