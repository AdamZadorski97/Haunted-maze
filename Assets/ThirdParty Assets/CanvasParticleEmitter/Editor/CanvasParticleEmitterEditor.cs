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
	[CustomEditor (typeof(CanvasParticleEmitter), true)]
	public class CanvasParticleEmitterEditor : GraphicEditor
	{

		private static string ARRAY_SIZE = "m_TextureArray.Array.size";
		private static string ARRAY_DATA = "m_TextureArray.Array.data[{0}]";

		private SerializedObject m_Object;
		private CanvasParticleEmitter myScript;

		//editor update
		private float emiterEditorPreviousTime = 0;
		private float emiterEditorDeltaTime;
		private SerializedProperty m_playOnEditor;
		private SerializedProperty m_playWhenSelected;
		private SerializedProperty m_isPlayingOnEditor;
		private SerializedProperty m_updateTargetIndex;

		//emitter properties
		private SerializedProperty m_playOnAwake;
		private SerializedProperty m_Duration;
		private SerializedProperty m_AutoKill;
		private SerializedProperty m_worldSpace;
		private SerializedProperty m_MaxParticles;
		private SerializedProperty m_ParticlesPerEmission;
		private SerializedProperty m_EmitterRate;
		private SerializedProperty m_EmitterSpread;
		private SerializedProperty m_EmitterDirection;
		private SerializedProperty m_EmitterShape;
		private SerializedProperty m_EmitterRectType;
		private SerializedProperty m_EmitterRect;
		private SerializedProperty m_EmitterRectTransform;
		private SerializedProperty m_EmitterCircleType;
		private SerializedProperty m_EmitterCircleRadius;
		private float m_EmitterCircleFloatRadius;
		private SerializedProperty m_EmitterCircleSegments;
		private SerializedProperty m_EmitterLinePointA;
		private SerializedProperty m_EmitterLinePointB;
		private SerializedProperty m_EmitterShapePointListPoints;
		private int emitterPointListSize;
		private SerializedProperty m_EmitterShapeImage;

		private SerializedProperty m_Gravity;
		private SerializedProperty m_Wave;
		private SerializedProperty m_WaveAmplitude;
		private SerializedProperty m_Turbulence;
		private SerializedProperty m_TurbulenceAmplitude;

		private SerializedProperty m_numberOfParticles;
		private SerializedProperty m_isPlaying;
		private SerializedProperty m_isActive;

		private SerializedProperty m_ParticleLife;
		private SerializedProperty m_ParticleLifeRandMin;
		private SerializedProperty m_ParticleLifeRandMax;
		private SerializedProperty m_particleSize;
		private SerializedProperty m_particleSizeCurve;
		private SerializedProperty m_ParticleSizeRandomMax;
		private SerializedProperty m_particleAspectRatio;
		private SerializedProperty m_ParticleSpeed;
		private SerializedProperty m_ParticleSpeedRandMin;
		private SerializedProperty m_ParticleSpeedRandMax;

		private SerializedProperty m_AlignedRotation;
		private SerializedProperty m_Stretchable;
		private SerializedProperty m_StretchableMultiplier;
		private SerializedProperty m_ParticleAngle;
		private SerializedProperty m_ParticleAngleRandMin;
		private SerializedProperty m_ParticleAngleRandMax;
		private SerializedProperty m_ParticleSpinVel;
		private SerializedProperty m_ParticleSpinVelRandMin;
		private SerializedProperty m_ParticleSpinVelRandMax;
		private SerializedProperty m_ParticleColor;
		private SerializedProperty m_useLifeColor;
		private SerializedProperty m_ParticleColorRamp;
		private SerializedProperty m_ParticleColorArray;
		private SerializedProperty m_ParticleColorRandonR;
		private SerializedProperty m_ParticleColorRandonG;
		private SerializedProperty m_ParticleColorRandonB;
		private SerializedProperty m_particleAlpha;
		private SerializedProperty m_particleAlphaCurve;
		private SerializedProperty m_TextureArray;
		private SerializedProperty m_NumOfTextures;
		private SerializedProperty m_TextureAnimable;
		private SerializedProperty m_TextureFrameRate;
		private SerializedProperty m_TextureRandomStartFrame;
		private SerializedProperty m_particleTextureAspectRatio;
		private SerializedProperty m_Material;
		//		private SerializedProperty m_ParticleColliders;

		protected static bool PointListFoldout = true;
		protected static bool FieldsFoldout = true;
		protected static bool ParticleLifeSpanFoldout = true;
		protected static bool ParticleSizeFoldout = true;
		protected static bool ParticleSpeedFoldout = true;
		protected static bool ParticleOrientationFoldout = true;
		protected static bool ParticleColorFoldout = true;


		protected override void OnEnable ()
		{
			base.OnEnable ();
	
			m_Object = new SerializedObject (target);
			myScript = (CanvasParticleEmitter)target;

			//read only
			m_numberOfParticles = m_Object.FindProperty ("m_numberOfParticles");
			m_isPlaying = m_Object.FindProperty ("m_isPlaying");
			m_isActive = m_Object.FindProperty ("m_isActive");

			//emitter base
			m_playOnAwake = m_Object.FindProperty ("m_playOnAwake");
			m_AutoKill = m_Object.FindProperty ("m_AutoKill");
			m_worldSpace = m_Object.FindProperty("m_worldSpace");
			m_EmitterRate = m_Object.FindProperty ("m_EmitterRate");
			m_Duration = m_Object.FindProperty ("m_Duration");
			m_MaxParticles = m_Object.FindProperty ("m_MaxParticles");
			m_ParticlesPerEmission = m_Object.FindProperty ("m_ParticlesPerEmission");


			//emitter shape
			m_EmitterShape = m_Object.FindProperty ("m_EmitterShape");

			//directional emitter
			m_EmitterSpread = m_Object.FindProperty ("m_EmitterSpread");
			m_EmitterDirection = m_Object.FindProperty ("m_EmitterDirection");
			
			//rect emitter
			m_EmitterRectType = m_Object.FindProperty ("m_EmitterShapeRectType");
			m_EmitterRect = m_Object.FindProperty ("m_EmitterShapeRect");
			m_EmitterRectTransform = m_Object.FindProperty ("m_EmitterShapeRectTransform");
			
			//circle emitter
			m_EmitterCircleType = m_Object.FindProperty ("m_EmitterShapeCircleType");
			m_EmitterCircleRadius	= m_Object.FindProperty ("m_EmitterShapeCircleRadius");
			m_EmitterCircleSegments	= m_Object.FindProperty ("m_EmitterShapeCircleSegments");
			
			//line emitter
			m_EmitterLinePointA = m_Object.FindProperty ("m_EmitterShapeLinePointA");
			m_EmitterLinePointB = m_Object.FindProperty ("m_EmitterShapeLinePointB");

			// point list emitter m_EmitterShapePointListPoints
			m_EmitterShapePointListPoints = m_Object.FindProperty ("m_EmitterShapePointListPoints");

			//image emitter
			m_EmitterShapeImage = m_Object.FindProperty ("m_EmitterShapeImage");
			//m_EmitterImagePixels = m_Object.FindProperty ("m_EmitterImagePixels");


			//forces
			m_Gravity = m_Object.FindProperty ("m_Gravity");
			m_Wave = m_Object.FindProperty ("m_Wave");
			m_WaveAmplitude = m_Object.FindProperty ("m_WaveAmplitude");
			m_Turbulence = m_Object.FindProperty ("m_Turbulence");
			m_TurbulenceAmplitude = m_Object.FindProperty ("m_TurbulenceAmplitude");


			//particle size and shape
			m_particleSize = m_Object.FindProperty ("m_particleSize");
			m_particleSizeCurve = m_Object.FindProperty ("m_particleSizeCurve");
			m_ParticleSizeRandomMax = m_Object.FindProperty ("m_ParticleSizeRandomMax");
			m_particleAspectRatio = m_Object.FindProperty ("m_particleAspectRatio");

			//particle behavior
			m_ParticleLife = m_Object.FindProperty ("m_ParticleLife");
			m_ParticleLifeRandMin = m_Object.FindProperty ("m_ParticleLifeRandMin");
			m_ParticleLifeRandMax = m_Object.FindProperty ("m_ParticleLifeRandMax");

			m_ParticleSpeed = m_Object.FindProperty ("m_ParticleSpeed");
			m_ParticleSpeedRandMin = m_Object.FindProperty ("m_ParticleSpeedRandMin");
			m_ParticleSpeedRandMax = m_Object.FindProperty ("m_ParticleSpeedRandMax");

			m_AlignedRotation = m_Object.FindProperty ("m_AlignedRotation");
			m_ParticleAngle = m_Object.FindProperty ("m_ParticleAngle");
			m_ParticleAngleRandMin = m_Object.FindProperty ("m_ParticleAngleRandMin");
			m_ParticleAngleRandMax = m_Object.FindProperty ("m_ParticleAngleRandMax");
			m_Stretchable = m_Object.FindProperty ("m_Stretchable");
			m_StretchableMultiplier = m_Object.FindProperty ("m_StretchableMultiplier");


			m_ParticleSpinVel = m_Object.FindProperty ("m_ParticleSpinVel");
			m_ParticleSpinVelRandMin = m_Object.FindProperty ("m_ParticleSpinVelRandMin");
			m_ParticleSpinVelRandMax = m_Object.FindProperty ("m_ParticleSpinVelRandMax");

			//particle color and tranparency
			m_useLifeColor = m_Object.FindProperty ("m_useLifeColor");
			m_ParticleColor = m_Object.FindProperty ("m_ParticleColor");
			m_ParticleColorRamp = m_Object.FindProperty ("m_ParticleColorRamp");
			m_ParticleColorArray = m_Object.FindProperty ("m_ParticleColorArray");
			m_ParticleColorRandonR = m_Object.FindProperty ("m_ParticleColorRandonR");
			m_ParticleColorRandonG = m_Object.FindProperty ("m_ParticleColorRandonG");
			m_ParticleColorRandonB = m_Object.FindProperty ("m_ParticleColorRandonB");
			m_particleAlpha = m_Object.FindProperty ("m_ParticleAlpha");
			m_particleAlphaCurve = m_Object.FindProperty ("m_particleAlphaCurve");
			m_TextureArray = m_Object.FindProperty ("m_TextureArray");
			m_TextureAnimable = m_Object.FindProperty ("m_isAnimated");
			m_TextureFrameRate = m_Object.FindProperty ("m_ParticleFrameRate");
			m_TextureRandomStartFrame = m_Object.FindProperty ("m_isRandomStartFrame");
			m_particleTextureAspectRatio	= m_Object.FindProperty ("m_particleTextureAspectRatio");
			m_NumOfTextures = m_Object.FindProperty (ARRAY_SIZE);
			//m_ParticleColliders = m_Object.FindProperty("m_ParticleColliders");
			m_Material = m_Object.FindProperty("m_Material");

			//editor update
			m_playOnEditor = m_Object.FindProperty ("m_playOnEditor");
			m_playWhenSelected = m_Object.FindProperty ("m_playWhenSelected");
			m_isPlayingOnEditor = m_Object.FindProperty ("m_isPlayingOnEditor");
			m_updateTargetIndex = m_Object.FindProperty ("m_updateTargetIndex");


			if (m_TextureArray.arraySize > 1) {
				myScript.CreateTextureAtlas ();
			}

			if (!Application.isPlaying){
				if (m_playOnEditor.boolValue == false) {
					RemoveUpdateTarget (m_updateTargetIndex.intValue);
					m_isPlayingOnEditor.boolValue = false;
					m_playWhenSelected.boolValue = false;
				}

				if (m_playOnEditor.boolValue == true && m_playWhenSelected.boolValue == true) {
					AddUpdateTarget (m_updateTargetIndex.intValue);
					m_isPlayingOnEditor.boolValue = true;
				}
			}
		}

		protected override void OnDisable ()
		{
			base.OnDisable ();
			if (!Application.isPlaying){
				if (m_playOnEditor.boolValue == true && m_playWhenSelected.boolValue == true) {
					RemoveUpdateTarget (m_updateTargetIndex.intValue);
					myScript.StopEmitter ();
				}
				if (m_playOnEditor.boolValue == false) {
					RemoveUpdateTarget (m_updateTargetIndex.intValue);
					myScript.StopEmitter ();
				}
			}
		}

		private void AddUpdateTarget (int ID)
		{
			if (Application.isPlaying == true)
				return;

			foreach (KeyValuePair<int, EditorApplication.CallbackFunction> kvp in CanvasParticleHelper.updateFunctionDic) {
				if (kvp.Key == m_updateTargetIndex.intValue) {
					return;
				}
			}
			EditorApplication.CallbackFunction updateCallback = EmitterUpdate;
			EditorApplication.update += updateCallback;
			CanvasParticleHelper.updateFunctionDic.Add (ID, updateCallback);
			m_updateTargetIndex.intValue = ID;
			m_isPlayingOnEditor.boolValue = true;
		}

		private void RemoveUpdateTarget (int ID)
		{
			if (Application.isPlaying == true)
				return;

			foreach (KeyValuePair<int, EditorApplication.CallbackFunction> kvp in CanvasParticleHelper.updateFunctionDic) {
				if (kvp.Key == ID) {
					EditorApplication.update -= kvp.Value;
					CanvasParticleHelper.updateFunctionDic.Remove (ID);
					m_isPlayingOnEditor.boolValue = false;
					return;
				}
			}
		}


		private void EmitterUpdate ()
		{
			if (Application.isPlaying == true)
				return;
			if (m_isPlayingOnEditor.boolValue == false)
				return;

			EditorUtility.SetDirty (m_Object.targetObject);
			emiterEditorDeltaTime = Time.realtimeSinceStartup - emiterEditorPreviousTime;

			myScript.UpdateFromEditor ();
			myScript.EditorEmitterTime = emiterEditorDeltaTime;

			emiterEditorPreviousTime = Time.realtimeSinceStartup;
		}

		public override bool HasPreviewGUI ()
		{
			return true;
		}

		public override void OnInspectorGUI ()
		{
			m_Object.Update ();

			//emitter properties
			EditorGUILayout.LabelField ("EMITTER STATS",EditorStyles.boldLabel);
			++EditorGUI.indentLevel;
			{
				EditorGUILayout.LabelField ("Particles number: ", m_numberOfParticles.intValue.ToString ());
				if (m_MaxParticles.intValue > 0)
					ProgressBar ((float)m_numberOfParticles.intValue / (float)m_MaxParticles.intValue, "Number of particles");
				EditorGUILayout.LabelField ("Emiter is active: " + m_isActive.boolValue.ToString ());
				EditorGUILayout.LabelField ("Emiter is playing: " + m_isPlaying.boolValue.ToString ());
			
				//editor attributes
				if (!Application.isPlaying){
//					EditorGUI.BeginChangeCheck ();
					m_playOnEditor.boolValue = GUILayout.Toggle (m_playOnEditor.boolValue, "Play on editor", "Button");//, new GUIContent("Play on editor"));
					if (GUI.changed) {
						if (m_playOnEditor.boolValue == false) {
							RemoveUpdateTarget (m_updateTargetIndex.intValue);
							m_isPlayingOnEditor.boolValue = false;
							myScript.StopEmitter ();
						} else {
							AddUpdateTarget (m_updateTargetIndex.intValue);
							m_isPlayingOnEditor.boolValue = true;
						}
					}
//					EditorGUI.EndChangeCheck ();

					if (m_playOnEditor.boolValue == true) {
						m_playWhenSelected.boolValue = GUILayout.Toggle (m_playWhenSelected.boolValue, "Play only when selected", "Button");
					
						if (m_Duration.floatValue > 0){
							if (GUILayout.Button("Instant Play")){
								myScript.StartEmitter();
							}
						}
					}	
				}

			}
			--EditorGUI.indentLevel;

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.PropertyField (m_playOnAwake);
            EditorGUILayout.PropertyField(m_worldSpace);
			EditorGUILayout.PropertyField (m_EmitterRate);
			EditorGUILayout.PropertyField (m_ParticlesPerEmission);
			if (m_ParticlesPerEmission.intValue < 0) m_ParticlesPerEmission.intValue = 0;


			EditorGUILayout.PropertyField (m_MaxParticles);
			if (m_MaxParticles.intValue < 0){
				m_MaxParticles.intValue = 0;
			}
			EditorGUILayout.PropertyField (m_Duration);
			if (m_Duration.floatValue < 0){
				m_Duration.floatValue = 0;
			}
			if (m_Duration.floatValue > 0){
				EditorGUILayout.PropertyField(m_AutoKill);
			}

			EditorGUILayout.PropertyField (m_EmitterShape);
			switch ((CanvasParticleEmitter.EmitterShapes)m_EmitterShape.enumValueIndex) {
			case CanvasParticleEmitter.EmitterShapes.Directional:
				EmitterDirectionalLayout ();
				break;
			case CanvasParticleEmitter.EmitterShapes.Point:
				EmitterPointLayout ();
				break;
			case CanvasParticleEmitter.EmitterShapes.RectEmitter:
				EmitterRectLayout ();
				break;
			case CanvasParticleEmitter.EmitterShapes.CircleEmitter:
				EmitterCircleLayout ();
				break;
			case CanvasParticleEmitter.EmitterShapes.LineEmitter:
				EmitterLineLayout ();
				break;
			case CanvasParticleEmitter.EmitterShapes.PointList:
				EmitterPointListLayout ();
				break;
			case CanvasParticleEmitter.EmitterShapes.Image:
				EmitterImageLayout ();
				break;
			}


			FieldsFoldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), FieldsFoldout, "Force Fields", true);
			if (FieldsFoldout == true) {
				++EditorGUI.indentLevel;
				{
					EditorGUILayout.PropertyField (m_Gravity);
					EditorGUILayout.PropertyField (m_Wave, new GUIContent ("Wave frequency"));
					EditorGUILayout.PropertyField (m_WaveAmplitude, new GUIContent ("Wave amplitude"));
					EditorGUILayout.PropertyField (m_Turbulence, new GUIContent ("Turbulence frequency"));
					EditorGUILayout.PropertyField (m_TurbulenceAmplitude, new GUIContent ("Turbulence amplitude"));
				}
				--EditorGUI.indentLevel;
			}

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (" PARTICLE PROPERTIES ",EditorStyles.boldLabel);
			++EditorGUI.indentLevel;
			{
				ParticleLifeSpanFoldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), ParticleLifeSpanFoldout, "Particles Life Span", true);
				if (ParticleLifeSpanFoldout == true) {
					++EditorGUI.indentLevel;
					{
						EditorGUILayout.PropertyField (m_ParticleLife, new GUIContent ("Life"));
						if (m_ParticleLife.floatValue < 0)
							m_ParticleLife.floatValue = 0;
						EditorGUILayout.PropertyField (m_ParticleLifeRandMin, new GUIContent ("Min Random Life"));
						if (m_ParticleLifeRandMin.floatValue < 0)
							m_ParticleLifeRandMin.floatValue = 0;
						EditorGUILayout.PropertyField (m_ParticleLifeRandMax, new GUIContent ("Max Random Life"));				
						if (m_ParticleLifeRandMax.floatValue < 0)
							m_ParticleLifeRandMax.floatValue = 0;
					}
					--EditorGUI.indentLevel;
				}


				ParticleSizeFoldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), ParticleSizeFoldout, "Particles Size", true);
				if (ParticleSizeFoldout == true) {
					++EditorGUI.indentLevel;
					{
						EditorGUILayout.PropertyField (m_particleSize, new GUIContent ("Size"));
						EditorGUILayout.LabelField ("Life Size Multiplier");
						EditorGUILayout.PropertyField (m_particleSizeCurve, new GUIContent (""));
						EditorGUILayout.PropertyField (m_ParticleSizeRandomMax, new GUIContent ("Random Size"));
						EditorGUILayout.PropertyField (m_particleAspectRatio, new GUIContent ("Aspect ratio"));
						if (m_ParticleSizeRandomMax.floatValue < 0)
							m_ParticleSizeRandomMax.floatValue = 0;
					}
					--EditorGUI.indentLevel;
				}

				ParticleSpeedFoldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), ParticleSpeedFoldout, "Particles Speed", true);
				if (ParticleSpeedFoldout == true) {
					++EditorGUI.indentLevel;
					{
						EditorGUILayout.PropertyField (m_ParticleSpeed, new GUIContent ("Speed"));
						EditorGUILayout.PropertyField (m_ParticleSpeedRandMin, new GUIContent ("Min Random Speed"));
						EditorGUILayout.PropertyField (m_ParticleSpeedRandMax, new GUIContent ("Max Random Speed"));
					}
					--EditorGUI.indentLevel;
				}


				ParticleOrientationFoldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), ParticleOrientationFoldout, "Particles Orientation", true);
				if (ParticleOrientationFoldout == true) {
					++EditorGUI.indentLevel;
					{
						EditorGUILayout.PropertyField (m_AlignedRotation, new GUIContent ("Aligned Rotation"));
						if (m_AlignedRotation.boolValue == false) {

							EditorGUILayout.PropertyField (m_ParticleAngle, new GUIContent ("Angle"));
							EditorGUILayout.PropertyField (m_ParticleAngleRandMin, new GUIContent ("Min Random Angle"));
							EditorGUILayout.PropertyField (m_ParticleAngleRandMax, new GUIContent ("Max Random Angle"));
							EditorGUILayout.Space ();
							EditorGUILayout.PropertyField (m_ParticleSpinVel, new GUIContent ("Spin Velocity"));
							EditorGUILayout.PropertyField (m_ParticleSpinVelRandMin, new GUIContent ("Min Random Spin"));
							EditorGUILayout.PropertyField (m_ParticleSpinVelRandMax, new GUIContent ("Max Random Spin"));
						} else {
							++EditorGUI.indentLevel;
							EditorGUILayout.PropertyField (m_Stretchable, new GUIContent ("Stretchable"));
							if (m_Stretchable.boolValue == true) {
								++EditorGUI.indentLevel;
								EditorGUILayout.PropertyField (m_StretchableMultiplier, new GUIContent ("Multiplier"));
								--EditorGUI.indentLevel;
							}
							--EditorGUI.indentLevel;
						}
					}
					--EditorGUI.indentLevel;
				}

				ParticleColorFoldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), ParticleColorFoldout, "Particles Color / Texture", true);
				if (ParticleColorFoldout == true) {
					++EditorGUI.indentLevel;
					{
						EditorGUI.BeginChangeCheck ();
						EditorGUILayout.LabelField ("Particle Textures");
						for (int i = 0; i < m_NumOfTextures.intValue; i++) {
							GUILayout.BeginVertical ();
							GUILayout.BeginHorizontal ();
							GUILayout.Label ((i + 1).ToString (), GUILayout.Width (10));

							SerializedProperty textureRef = m_TextureArray.GetArrayElementAtIndex(i);
							textureRef.objectReferenceValue = EditorGUILayout.ObjectField(textureRef.objectReferenceValue, typeof(Texture), false);
							
							bool objEnabled = GUI.enabled;
							GUI.enabled &= CanMoveUp (i);
							if (GUILayout.Button ("-", GUILayout.Width (20))) {
								myScript.SwapTexture(i, i-1);
							}

							GUI.enabled = objEnabled && CanMoveDown (i);
							if (GUILayout.Button ("+", GUILayout.Width (20))) {
								myScript.SwapTexture(i, i+1);
							}

							GUI.enabled = objEnabled && m_TextureArray.arraySize > 1;
							if (GUILayout.Button ("X", GUILayout.Width (20))) {
								myScript.RemoveTexture(i);
							}

							GUI.enabled = objEnabled;
							GUILayout.EndHorizontal ();
							GUILayout.EndVertical ();

						}

						if (GUILayout.Button ("Add Texture")) {
							myScript.AddTexture(m_TextureArray.GetArrayElementAtIndex(m_TextureArray.arraySize-1).objectReferenceValue as Texture);
						}
						if(EditorGUI.EndChangeCheck ()){
							m_Object.ApplyModifiedProperties ();
							myScript.CreateTextureAtlas ();
							m_Object.ApplyModifiedProperties ();
						}

						EditorGUILayout.PropertyField(m_Material);


						EditorGUILayout.PropertyField (m_particleTextureAspectRatio, new GUIContent ("Use Texture Aspect Ratio"));		

						if (m_TextureArray.arraySize > 1) {
							EditorGUILayout.PropertyField (m_TextureAnimable, new GUIContent ("Animated Texture"));
						}

						if (m_TextureAnimable.boolValue == true) {
							EditorGUILayout.PropertyField (m_TextureRandomStartFrame, new GUIContent ("Random Start"));
							EditorGUILayout.PropertyField (m_TextureFrameRate, new GUIContent ("Frame Rate"));
						}



						EditorGUILayout.PropertyField (m_useLifeColor, new GUIContent ("Use Life Color"));
						if (m_useLifeColor.boolValue == true) {
							EditorGUILayout.LabelField ("Life Color");
							EditorGUILayout.PropertyField (m_ParticleColorRamp, new GUIContent (""));
						} else {
							EditorGUILayout.PropertyField (m_ParticleColor, new GUIContent ("Color"));
						}
						EditorGUILayout.PropertyField (m_ParticleColorRandonR, new GUIContent ("Random Red"));
						EditorGUILayout.PropertyField (m_ParticleColorRandonG, new GUIContent ("Random Green"));
						EditorGUILayout.PropertyField (m_ParticleColorRandonB, new GUIContent ("Random Blue"));
						ArrayGUI (m_Object, m_ParticleColorArray, "Color Array");
						EditorGUILayout.PropertyField (m_particleAlpha, new GUIContent ("Opacity"));
						EditorGUILayout.LabelField ("Life Opacity Multiplier");
						EditorGUILayout.PropertyField (m_particleAlphaCurve, new GUIContent (""));
					}
					--EditorGUI.indentLevel;
				}
				--EditorGUI.indentLevel;
			}
			m_Object.ApplyModifiedProperties ();
		}


		private Texture[] GetTexturesArray () {
			int arraySize = m_Object.FindProperty (ARRAY_SIZE).intValue;
			Texture[] imagesArray = new Texture[arraySize];
			for (int i = 0; i < arraySize; i++) {
				imagesArray [i] = m_Object.FindProperty (string.Format (ARRAY_DATA, i)).objectReferenceValue as Texture;
			}
			return imagesArray;
		}

		private void SetArrayTexture (int index, Texture texture) {
			m_Object.FindProperty (string.Format (ARRAY_DATA, index)).objectReferenceValue = texture;
			myScript.CreateTextureAtlas ();
		}

		private Texture GetArrayTexture (int index) {
			return m_Object.FindProperty (string.Format (ARRAY_DATA, index)).objectReferenceValue as Texture;
		}

		private void DeleteTexture (int index) {
			for (int i = index; i < m_NumOfTextures.intValue - 1; i++) {
				SetArrayTexture (i, GetArrayTexture (i + 1));
			}
			m_NumOfTextures.intValue--;
		}

		private void SwapTextures (int oldIndex, int newIndex)
		{
			Texture texture = GetArrayTexture (oldIndex);
			SetArrayTexture (oldIndex, GetArrayTexture (newIndex));
			SetArrayTexture (newIndex, texture);
		}

		private bool CanMoveUp (int index)
		{
			return index >= 1;
		}

		private bool CanMoveDown (int index)
		{
			return index < m_NumOfTextures.intValue - 1;
		}

		void ArrayGUI (SerializedObject obj, SerializedProperty property, string propertyName)
		{
			int size = property.arraySize;
			
			int newSize = EditorGUILayout.IntField (propertyName, size);
			
			if (newSize != size) {
				property.arraySize = newSize;
			}
			
			EditorGUI.indentLevel = 3;
			
			for (int i = 0; i < newSize; i++) {
				var prop = property.GetArrayElementAtIndex (i);
				EditorGUILayout.PropertyField (prop);
			}
			EditorGUI.indentLevel = 2;
		}


		private void EmitterPointLayout ()
		{
			
		}

		private void EmitterDirectionalLayout ()
		{
			++EditorGUI.indentLevel;
			{
				EditorGUILayout.PropertyField (m_EmitterDirection, new GUIContent ("Direction"));
				EditorGUILayout.PropertyField (m_EmitterSpread, new GUIContent ("Spread"));		
			}
			--EditorGUI.indentLevel;
		}

		private void EmitterRectLayout ()
		{
			++EditorGUI.indentLevel;
			{
				EditorGUILayout.PropertyField (m_EmitterRectType, new GUIContent ("Emitter Type"));
				EditorGUILayout.PropertyField (m_EmitterRect, new GUIContent ("Rect"));
				EditorGUILayout.PropertyField (m_EmitterRectTransform, new GUIContent ("RectTransform"));	
			}
			--EditorGUI.indentLevel;
		}

		private void EmitterCircleLayout ()
		{
			++EditorGUI.indentLevel;
			{
				EditorGUILayout.PropertyField (m_EmitterCircleType, new GUIContent ("Emitter Type"));
				EditorGUILayout.PropertyField (m_EmitterCircleRadius, new GUIContent ("Radius"));
				EditorGUILayout.PropertyField (m_EmitterCircleSegments, new GUIContent ("Segments"));
			}
			--EditorGUI.indentLevel;
		}

		private void EmitterLineLayout ()
		{
			++EditorGUI.indentLevel;
			{
				EditorGUILayout.PropertyField (m_EmitterLinePointA, new GUIContent ("First Point"));	
				EditorGUILayout.PropertyField (m_EmitterLinePointB, new GUIContent ("Second Point"));	
				EditorGUILayout.PropertyField (m_EmitterDirection, new GUIContent ("Direction"));
				EditorGUILayout.PropertyField (m_EmitterSpread, new GUIContent ("Spread"));
			}
			--EditorGUI.indentLevel;
		}

		private void EmitterPointListLayout ()
		{
			PointListFoldout = EditorGUI.Foldout (EditorGUILayout.GetControlRect (), PointListFoldout, "Point List", true);
			if (PointListFoldout == true) 
			{
				GUILayout.BeginVertical();
				for (int i = 0; i < m_EmitterShapePointListPoints.arraySize; i++) {
					GUILayout.BeginHorizontal();
					SerializedProperty pointRef = m_EmitterShapePointListPoints.GetArrayElementAtIndex (i);
					++EditorGUI.indentLevel;
					{
						GUILayout.Label (("Point " + (i + 1)).ToString (), GUILayout.Width (50));
						float pointX = pointRef.vector2Value.x;
						float pointY = pointRef.vector2Value.y;
						pointX = EditorGUILayout.FloatField (pointX);
						pointY = EditorGUILayout.FloatField (pointY);
						pointRef.vector2Value = new Vector2 (pointX, pointY);

						if (GUILayout.Button ("x")) {
							myScript.RemoveEmitterPoint (i);
						}
					}
					--EditorGUI.indentLevel;
					GUILayout.EndHorizontal();
				}
				GUILayout.EndVertical();
			}

			if (GUILayout.Button ("Add Point")) {
				myScript.AddEmitterPoint();
			}
		}

		private void EmitterImageLayout()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField (m_EmitterShapeImage, new GUIContent ("Image to Emit"));	
			if(EditorGUI.EndChangeCheck ()){
				m_Object.ApplyModifiedProperties ();
				myScript.CreateParticlePixels ();
				m_Object.ApplyModifiedProperties ();
			}
		}


		private void ProgressBar (float value, string label)
		{
			Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
			EditorGUI.ProgressBar (rect, value, label);
			EditorGUILayout.Space ();
		}


		private void OnSceneGUI ()
		{
			CanvasParticleEmitter EmitterObject = target as CanvasParticleEmitter;

			switch ((CanvasParticleEmitter.EmitterShapes)m_EmitterShape.enumValueIndex) {
			case CanvasParticleEmitter.EmitterShapes.Directional:
				DrawGizmoDirectional (EmitterObject);
				break;
			case CanvasParticleEmitter.EmitterShapes.Point:
				DrawGizmoPoint (EmitterObject);
				break;
			case CanvasParticleEmitter.EmitterShapes.RectEmitter:
				DrawGizmoRect (EmitterObject);
				break;
			case CanvasParticleEmitter.EmitterShapes.CircleEmitter:
				DrawGizmoCircle (EmitterObject);
				break;
			case CanvasParticleEmitter.EmitterShapes.LineEmitter:
				DrawGizmoLine (EmitterObject);
				break;
			case CanvasParticleEmitter.EmitterShapes.PointList:
				DrawGizmoPoints (EmitterObject);
				break;
			}
		}

		private void DrawGizmoPoint (CanvasParticleEmitter EmitterObject)
		{
			Handles.DrawLine (EmitterObject.transform.position + new Vector3 (0, -1, 0), EmitterObject.transform.position + new Vector3 (0, 1, 0));
			Handles.DrawLine (EmitterObject.transform.position + new Vector3 (-1, 0, 0), EmitterObject.transform.position + new Vector3 (1, 0, 0));
		}

		private void DrawGizmoDirectional (CanvasParticleEmitter EmitterObject)
		{
			Handles.DrawLine (EmitterObject.transform.position, RotatePointAroundPivot (EmitterObject.transform.position + new Vector3 (100, 0, 0), EmitterObject.transform.position, new Vector3 (0, 0, -m_EmitterDirection.floatValue - (m_EmitterSpread.floatValue / 4) + 90)));
			Handles.DrawLine (EmitterObject.transform.position, RotatePointAroundPivot (EmitterObject.transform.position + new Vector3 (100, 0, 0), EmitterObject.transform.position, new Vector3 (0, 0, -m_EmitterDirection.floatValue + (m_EmitterSpread.floatValue / 4) + 90)));
		}

		private void DrawGizmoCircle (CanvasParticleEmitter EmitterObject)
		{
			Vector3[] circlePoints = CirclePoints (EmitterObject.transform.position, m_EmitterCircleSegments.intValue, m_EmitterCircleRadius.floatValue).ToArray ();
			Handles.DrawPolyLine (circlePoints);
			Handles.DrawLine (circlePoints [circlePoints.Length - 1], circlePoints [0]);
		}

		private void DrawGizmoRect (CanvasParticleEmitter EmitterObject)
		{
			Vector3[] rectPoints = RectPoints (m_EmitterRect.rectValue, EmitterObject.transform.position);
			Handles.DrawPolyLine (rectPoints);
			Handles.DrawLine (rectPoints [3], rectPoints [0]);
		}

		private void DrawGizmoLine (CanvasParticleEmitter EmitterObject)
		{
			Vector3 lineCenter = EmitterObject.transform.position + (new Vector3 (m_EmitterLinePointA.vector2Value.x, m_EmitterLinePointA.vector2Value.y, 0) + new Vector3 (m_EmitterLinePointB.vector2Value.x, m_EmitterLinePointB.vector2Value.y, 0)) / 2;
			Handles.DrawLine (new Vector3 (m_EmitterLinePointA.vector2Value.x, m_EmitterLinePointA.vector2Value.y, 0) + EmitterObject.transform.position, new Vector3 (m_EmitterLinePointB.vector2Value.x, m_EmitterLinePointB.vector2Value.y, 0) + EmitterObject.transform.position);
			Handles.DrawLine (lineCenter, RotatePointAroundPivot (lineCenter + new Vector3 (100, 0, 0), lineCenter, new Vector3 (0, 0, -m_EmitterDirection.floatValue - (m_EmitterSpread.floatValue / 2) + 90)));
			Handles.DrawLine (lineCenter, RotatePointAroundPivot (lineCenter + new Vector3 (100, 0, 0), lineCenter, new Vector3 (0, 0, -m_EmitterDirection.floatValue + (m_EmitterSpread.floatValue / 2) + 90)));
		}

		private void DrawGizmoPoints (CanvasParticleEmitter EmitterObject)
		{
			Handles.DrawLine (EmitterObject.transform.position + new Vector3 (0, -1, 0), EmitterObject.transform.position + new Vector3 (0, 1, 0));
			Handles.DrawLine (EmitterObject.transform.position + new Vector3 (-1, 0, 0), EmitterObject.transform.position + new Vector3 (1, 0, 0));
			for (int i = 0; i < m_EmitterShapePointListPoints.arraySize; i++) {
				Vector3 lineCenter = EmitterObject.transform.position + (new Vector3 (m_EmitterShapePointListPoints.GetArrayElementAtIndex (i).vector2Value.x, m_EmitterShapePointListPoints.GetArrayElementAtIndex (i).vector2Value.y, 0));
				Handles.DrawLine (lineCenter - new Vector3 (1, 0, 0), lineCenter + new Vector3 (1, 0, 0));
				Handles.DrawLine (lineCenter - new Vector3 (0, 1, 0), lineCenter + new Vector3 (0, 1, 0));
			}
		}

		private Vector3 RotatePointAroundPivot (Vector3 point, Vector3 pivot, Vector3 angles)
		{
			Vector3 dir = point - pivot; 
			dir = Quaternion.Euler (angles) * dir; 
			point = dir + pivot; 
			return point; 
		}

		private Vector3[] RectPoints (Rect rect, Vector3 center)
		{
			Vector3[] rectPoints = new Vector3[4];

			rectPoints [0] = new Vector3 (rect.xMin, rect.yMin, 0) + center;
			rectPoints [1] = new Vector3 (rect.xMax, rect.yMin, 0) + center;
			rectPoints [2] = new Vector3 (rect.xMax, rect.yMax, 0) + center;
			rectPoints [3] = new Vector3 (rect.xMin, rect.yMax, 0) + center;

			return rectPoints;
		}

		private List<Vector3> CirclePoints (Vector3 center, int sides, float radius)
		{
			List<Vector3> circleVertices = new List<Vector3> ();
			float x, y, t;
			for (int i = 0; i < sides; i++) {
				t = 2 * Mathf.PI * ((float)i / (float)sides); 
				x = Mathf.Cos (t) * radius;
				y = Mathf.Sin (t) * radius;
				
				Vector3 vertice = new Vector3 (x, y, 0);
				vertice += center;
				circleVertices.Add (vertice);
			}
			return circleVertices;
		}


	}
}
