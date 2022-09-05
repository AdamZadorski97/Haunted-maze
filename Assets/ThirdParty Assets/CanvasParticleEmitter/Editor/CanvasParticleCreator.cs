using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEditor.UI
{

	public class CanvasParticleCreator: MonoBehaviour {

	[MenuItem("GameObject/UI/Canvas Particle Emitter", false, 2011)]
	public static void AddCanvasParticleEmitter () {
			GameObject parent = Selection.activeObject as GameObject;
			if (parent == null || parent.GetComponentInParent<Canvas>() == null)
			{
				parent = GetOrCreateCanvasGameObject();
			}

			GameObject particleEmitterObject = new GameObject("Canvas Particle Emitter");
			GameObjectUtility.SetParentAndAlign(particleEmitterObject, parent);
			RectTransform rectTransform = particleEmitterObject.AddComponent<RectTransform>();
			rectTransform.sizeDelta = Vector2.one;

			particleEmitterObject.AddComponent<CanvasRenderer>();
			particleEmitterObject.AddComponent<CanvasParticleEmitter>();

			Selection.activeGameObject = particleEmitterObject;
	}


		static public GameObject GetOrCreateCanvasGameObject()
		{
			GameObject selectedGo = Selection.activeGameObject;
			
			// Try to find a gameobject that is the selected GO or one if its parents.
			Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
			if (canvas != null && canvas.gameObject.activeInHierarchy)
				return canvas.gameObject;
			
			// No canvas in selection or its parents? Then use just any canvas..
			canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
			if (canvas != null && canvas.gameObject.activeInHierarchy)
				return canvas.gameObject;
			
			// No canvas in the scene at all? Then create a new one.
			return CreateNewUI();
		}

		static public GameObject CreateNewUI()
		{
			// Root for the UI
			var root = new GameObject("Canvas");
			root.layer = LayerMask.NameToLayer("UI");
			Canvas canvas = root.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			root.AddComponent<CanvasScaler>();
			root.AddComponent<GraphicRaycaster>();
			Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

			// if there is no event system add one...
			//CreateEventSystem(false);
			return root;
		}

}
}