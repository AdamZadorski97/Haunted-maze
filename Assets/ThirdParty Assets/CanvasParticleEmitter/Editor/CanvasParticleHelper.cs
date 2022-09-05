using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CanvasParticleHelper : MonoBehaviour {


	public static List<EditorApplication.CallbackFunction> updateFunctionList = new List<EditorApplication.CallbackFunction>();

	public static Dictionary<int, EditorApplication.CallbackFunction> updateFunctionDic = new Dictionary<int, EditorApplication.CallbackFunction>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
