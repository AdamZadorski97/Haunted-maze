using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasParticleHelper {


	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot;
		dir = Quaternion.Euler(angles)*dir; 
		point = dir + pivot;
		return point; 
	}

	public static Vector2 GetPointInSegment(Vector2 pointA, Vector2 pointB, float percent)
	{
		Vector2 newPoint;
		newPoint = new Vector2( pointA.x + (percent * (pointB.x - pointA.x)), pointA.y + (percent * (pointB.y - pointA.y)));
		return newPoint;
	}


	public static Rect GetRectCanvasSpace(RectTransform rectTransform)
	{
		Vector3[] corners = new Vector3[4];
		Vector3[] screenCorners = new Vector3[2];
		Canvas canvas = GetCanvas(rectTransform.gameObject);
		rectTransform.GetWorldCorners(corners);

		if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
		{
			screenCorners[0] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[1]);
			screenCorners[1] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[3]);
		}
		else
		{
			screenCorners[0] = RectTransformUtility.WorldToScreenPoint(null, corners[1]);
			screenCorners[1] = RectTransformUtility.WorldToScreenPoint(null, corners[3]);
		}
		float xMin = screenCorners[0].x - (canvas.GetComponent<RectTransform>().sizeDelta.x * rectTransform.pivot.x);
		float yMin = screenCorners[0].y - (canvas.GetComponent<RectTransform>().sizeDelta.y * rectTransform.pivot.y);
		float xMax = screenCorners[1].x - screenCorners[0].x;
		float yMax = screenCorners[1].y - screenCorners[0].y;
		return new Rect(xMin, yMin, xMax, yMax);
	}


	public static Canvas GetCanvas(GameObject gameObject)
	{
		Canvas canvas = (gameObject != null) ? gameObject.GetComponentInParent<Canvas>() : null;
		if (canvas != null && canvas.gameObject.activeInHierarchy)
			return canvas;

		canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
		if (canvas != null && canvas.gameObject.activeInHierarchy)
			return canvas;

		return null;
	}


	public static List<Vector2> CircleShape(int sides, float radius)
	{
		List<Vector2> circleVertices = new List<Vector2>();
		float x, y, t;
		for (int i = 0; i < sides; i++){
			t = 2 * Mathf.PI * ((float)i / (float)sides); 
			x = Mathf.Cos(t) * radius;
			y = Mathf.Sin(t) * radius;

			Vector2 vertice = new Vector2(x, y);
			circleVertices.Add(vertice);
		}
		return circleVertices;
	}



	public static Texture2D CreateTextureAtlas(Texture[] textureArray){
		float TilesOffset = 2;
		List<int> validTextures = new List<int>();
		Vector2 TextureAtlasSize = new Vector2(TilesOffset, TilesOffset);
		List<Vector4> m_AtlasUVs = new List<Vector4>();

		float greaterHeight = 0;
		for (int i = 0; i < textureArray.Length; i++){
			if (textureArray != null){
				if (textureArray[i] != null){
					if (greaterHeight < textureArray[i].height) greaterHeight = textureArray[i].height + (TilesOffset * 2);
					TextureAtlasSize = new Vector2(TextureAtlasSize.x + textureArray[i].width + TilesOffset, greaterHeight);
					validTextures.Add(i);
				}
			}
		}

		Texture2D newTextureAtlas = new Texture2D(Mathf.RoundToInt(TextureAtlasSize.x), Mathf.RoundToInt(TextureAtlasSize.y), TextureFormat.ARGB32, false);
		Color fillColor = Color.clear;
		Color[] fillPixels = new Color[newTextureAtlas.width * newTextureAtlas.height];
		for (int i = 0; i < fillPixels.Length; i++)	{
			fillPixels[i] = fillColor;
		}
		newTextureAtlas.SetPixels(fillPixels);

		int currentXPosition = Mathf.RoundToInt(TilesOffset);
		for (int i = 0; i < validTextures.Count; i++){
			RenderTexture previous = RenderTexture.active;
			Texture currentTexture = textureArray[validTextures[i]];
			RenderTexture tempReadableTexture = RenderTexture.GetTemporary( currentTexture.width, currentTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
			Graphics.Blit(currentTexture, tempReadableTexture);
			RenderTexture.active = tempReadableTexture;
			newTextureAtlas.ReadPixels(new Rect(0, 0, tempReadableTexture.width, tempReadableTexture.height), currentXPosition, Mathf.RoundToInt(TilesOffset));
			newTextureAtlas.Apply();


			Vector4 newUV = new Vector4( (currentXPosition - TilesOffset/2)/newTextureAtlas.width, 0, (currentXPosition - TilesOffset/2 + textureArray[i].width)/newTextureAtlas.width, 1);
			m_AtlasUVs.Add(newUV);

			RenderTexture.ReleaseTemporary(tempReadableTexture);
			RenderTexture.active = previous;
			currentXPosition += currentTexture.width + Mathf.RoundToInt(TilesOffset);
		}

		return newTextureAtlas;
	}

}
