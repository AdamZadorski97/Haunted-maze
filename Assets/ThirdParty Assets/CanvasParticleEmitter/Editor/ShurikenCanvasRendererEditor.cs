using UnityEditor;
using UnityEditor.UI;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace UnityEditor.UI
{
	[CustomEditor (typeof(ShurikenCanvasRenderer), true)]
	public class ShurikenCanvasRendererEditor : GraphicEditor
	{

		private SerializedObject m_Object;
		private ShurikenCanvasRenderer myScript;

		private SerializedProperty m_sizeMultiplier;

		protected override void OnEnable ()
		{
			base.OnEnable ();
	
			m_Object = new SerializedObject (target);
			myScript = (ShurikenCanvasRenderer)target;
			m_sizeMultiplier = m_Object.FindProperty ("m_sizeMultiplier");
		}

		protected override void OnDisable ()
		{
			base.OnDisable ();
		}


		public override bool HasPreviewGUI ()
		{
			return true;
		}

		public override void OnInspectorGUI ()
		{
			m_Object.Update ();
			EditorGUILayout.PropertyField (m_sizeMultiplier);
			m_Object.ApplyModifiedProperties ();
		}


		private void OnSceneGUI ()
		{
			ShurikenCanvasRenderer EmitterObject = target as ShurikenCanvasRenderer;
			DrawGizmoPoint (EmitterObject);
		}

		private void DrawGizmoPoint (ShurikenCanvasRenderer EmitterObject)
		{
			Handles.DrawLine (EmitterObject.transform.position + new Vector3 (0, -1, 0), EmitterObject.transform.position + new Vector3 (0, 1, 0));
			Handles.DrawLine (EmitterObject.transform.position + new Vector3 (-1, 0, 0), EmitterObject.transform.position + new Vector3 (1, 0, 0));
		}

	}
}
