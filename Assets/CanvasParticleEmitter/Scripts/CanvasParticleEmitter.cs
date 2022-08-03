// Canvvas particle emitter 1.5

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.CoroutineTween;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
	public class CanvasParticle {

		public Vector2 ParticlePosition;
		public float ParticleSize;
		public float ParticleRandomSize;
		public Vector2 ParticleVelocity;
		public Vector2[] ParticleVertices;
		public CanvasParticleEmitter ParticleEmiter;
		public float ParticleLife;
		public float ParticleTotalLife;
		public float ParicleRotation;
		public float ParticleSpinVel;
		public Color ParticleColor;
		public Color ParticleOriginalColor;
		public float ParticleAlpha;
		public Texture ParticleTexture;
		public float ParticleAspectRatio;
		public System.Collections.Generic.List <Texture> ParticleTextureAnim;
		public int ParticleTextureFrame;
		public int ParticleTextureFrameRate;
		public float ParticleAnimTime;
		public Rect ParticleRect;
		public bool hasCollided;
	}

	public class PixelParticle{
		public Vector2 PixelPosition;
		public Color PixelColor;
	}
			
	[AddComponentMenu("UI/Canvas Particle Emitter", 16)]
	public class CanvasParticleEmitter : MaskableGraphic, ISerializationCallbackReceiver {

		// editor play
		[SerializeField] private bool m_playOnEditor = false;
		public bool PlayOnEditor { get { return m_playOnEditor;} set { m_playOnEditor = value;}}

		[SerializeField] private bool m_playWhenSelected = true;
		public bool PlayWhenSelected { get { return m_playWhenSelected;} set { m_playWhenSelected = value;}}

		[SerializeField] private bool m_isPlayingOnEditor = false;
		public bool isPlayingOnEditor { get { return m_isPlayingOnEditor;} set { m_isPlayingOnEditor = value;}}

		[SerializeField] private int m_updateTargetIndex;
		//


		[SerializeField] private bool m_playOnAwake = true;
		public bool PlayOnAwake { get { return m_playOnAwake;} set { m_playOnAwake = value;}}

		[SerializeField] private bool m_isActive = true;
		public bool IsEmitterActive { get { return m_isActive; }}

		[SerializeField] private bool m_isPlaying = true; 
		public bool IsEmitterPlaying { get {return m_isPlaying; }}

		[SerializeField] private float m_Duration = 0;
		public float Duration { get{return m_Duration;} set{m_Duration = value;} }

		[SerializeField] private bool m_AutoKill = true;
		public bool AutoKill { get { return m_AutoKill; } set { m_AutoKill = value; } }

		[SerializeField] private bool m_worldSpace = true;
		public bool WorldSpace { get { return m_worldSpace; } set { m_worldSpace = value; } }
		private Vector3 m_objectPosition;

		[SerializeField] private float m_elapsedEmitterTime = 0;
		public float ElapsedEmitterTime { get {return m_elapsedEmitterTime;} }

		float m_lastEditorTimeSinceStartUp = 0;
		[SerializeField] private float m_editorEmitterTime = 0;
		public float EditorEmitterTime { get {return m_editorEmitterTime;} set { m_editorEmitterTime = value; } }
		
		[SerializeField] private int m_MaxParticles = 0;
		public int MaxParticles { get {return m_MaxParticles;} set{m_MaxParticles = value;} }

		[SerializeField] private int m_numberOfParticles = 0;
		public int NumberOfParticles { get { return m_numberOfParticles; } }

		[SerializeField] private int m_ParticlesPerEmission = 10;
		public int ParticlesPerEmission { get{return m_ParticlesPerEmission;} set{m_ParticlesPerEmission = value;} }

		[SerializeField] private float m_EmitterRate = 10f;
		public float EmiterRate { get{return m_EmitterRate;} set{m_EmitterRate = value;} }

		[RangeAttribute(0, 360)]
		[SerializeField] private float m_EmitterSpread = 20f;
		public float EmitterSpread { get{return m_EmitterSpread;} set{m_EmitterSpread = value;} }

		[RangeAttribute(0, 360)]
		[SerializeField] private float m_EmitterDirection = 0;
		public float EmitterDirection { get{return m_EmitterDirection;} set{m_EmitterDirection = value;} }

		[SerializeField] private Vector2 m_Gravity = Vector2.zero;
		public Vector2 Gravity { get{return m_Gravity;} set{m_Gravity = value;} }
		
		[SerializeField] private Vector2 m_Wave = new Vector2(100, 100);
		public Vector2 Wave { get{return m_Wave;} set{m_Wave = value;} }
		
		[SerializeField] private Vector2 m_WaveAmplitude = Vector2.zero;
		public Vector2 WaveAmplitude { get{return m_WaveAmplitude;} set{m_WaveAmplitude = value;} }
		
		[SerializeField] private Vector2 m_Turbulence = new Vector2(100, 100);
		public Vector2 Turbulence { get{return m_Turbulence;} set{m_Turbulence = value;} }
		
		[SerializeField] private Vector2 m_TurbulenceAmplitude = Vector2.zero;
		public Vector2 TurbulenceAmplitude { get{return m_TurbulenceAmplitude;} set{m_TurbulenceAmplitude = value;} }
		
		public enum EmitterShapes {
			Point,
			Directional,
			RectEmitter,
			CircleEmitter,
			LineEmitter,
			PointList,
			Image,
		}
		[SerializeField] private EmitterShapes m_EmitterShape;
		public EmitterShapes EmitterShape { get{return m_EmitterShape;} set{m_EmitterShape = value;} }


		public enum EmitterShapeLineTypes {
			Above,
			Positive,
			Negative,
			Both,
		}
		[SerializeField] private EmitterShapeLineTypes m_EmitterShapeLineType;
		public EmitterShapeLineTypes EmitterShapeLineType { get { return m_EmitterShapeLineType;} set { m_EmitterShapeLineType = value; }}
		[SerializeField] private Vector2 m_EmitterShapeLinePointA;
		public Vector2 EmitterShapeLinePointA { get{ return m_EmitterShapeLinePointA;} set {m_EmitterShapeLinePointA = value;}}
		[SerializeField] private Vector2 m_EmitterShapeLinePointB;
		public Vector2 EmitterShapeLinePointB { get{ return m_EmitterShapeLinePointB;} set {m_EmitterShapeLinePointB = value;}}
		[SerializeField] private float m_EmitterShapeLineLength;
		public float EmitterShapeLineLength { get{ return m_EmitterShapeLineLength;} set {m_EmitterShapeLineLength = value;}}
		[SerializeField] private float m_EmitterShapeLineAngle;
		public float EmitterShapeLineAngle { get { return m_EmitterShapeLineAngle;} set { m_EmitterShapeLineAngle = value;}}


		public enum EmitterShapeRectTypes {
			Area,
			Edges,
			Vertices
		}
		[SerializeField] private EmitterShapeRectTypes m_EmitterShapeRectType;
		public EmitterShapeRectTypes EmitterShapeRectType { get {return m_EmitterShapeRectType;} set {m_EmitterShapeRectType = value;}}
		[SerializeField] private  Rect m_EmitterShapeRect = new Rect(0, 0, 100, 100);
		public Rect EmitterShapeRect {get {return m_EmitterShapeRect;} set {m_EmitterShapeRect = value;}}
		[SerializeField] private  RectTransform m_EmitterShapeRectTransform;
		public RectTransform EmitterShapeRectTransform {get {return m_EmitterShapeRectTransform;} set {m_EmitterShapeRectTransform = value;}}


		//emitter type = circle
		public enum EmitterShapeCircleTypes {
			Edge,
			Vertices
		}
		[SerializeField] private EmitterShapeCircleTypes m_EmitterShapeCircleType;
		public EmitterShapeCircleTypes EmitterShapeCircleType { get {return m_EmitterShapeCircleType;} set {m_EmitterShapeCircleType = value;}}
		[SerializeField] private float m_EmitterShapeCircleRadius;
		public float EmitterShapeCircleRadius { get { return m_EmitterShapeCircleRadius; } set {m_EmitterShapeCircleRadius = value;}}
		[RangeAttribute(3, 100)]
		[SerializeField] private int m_EmitterShapeCircleSegments = 20;
		public int EmitterShapeCircleSegments { get { return m_EmitterShapeCircleSegments;} set { m_EmitterShapeCircleSegments = value;}}


		//emitter type = points
		[SerializeField] private List<Vector2> m_EmitterShapePointListPoints = new List<Vector2>(1);
		public List<Vector2> EmitterShapePointListPoints { get{ return m_EmitterShapePointListPoints;} set {m_EmitterShapePointListPoints = value;}}


		//emitter type = image
		[SerializeField] private Texture2D m_EmitterShapeImage = null;
		public Texture2D EmitterShapeImage { get{ return m_EmitterShapeImage;} set {m_EmitterShapeImage = value;}}

		private List<PixelParticle> m_EmitterImagePixels = new List<PixelParticle>();



		[SerializeField] private float m_ParticleLife = 3f;
		public float ParticleLife { get{return m_ParticleLife;} set{m_ParticleLife = value;} }

		[SerializeField] private float m_ParticleLifeRandMin = 0;
		public float ParticleLifeRandomMin { get {return m_ParticleLifeRandMin;} set{m_ParticleLifeRandMin = value;} }

		[SerializeField] private float m_ParticleLifeRandMax = 1f;
		public float ParticleLifeRandomMax { get{return m_ParticleLifeRandMax;} set{m_ParticleLifeRandMax = value;} }




		[SerializeField] private AnimationCurve m_particleSizeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
		public AnimationCurve ParticleSizeCurve { get {return m_particleSizeCurve;} set {m_particleSizeCurve = value;}}

		[SerializeField] private float m_particleSize = 10f;
		public float ParticleSize { get {return m_particleSize;} set {m_particleSize = value; if (value < 0) m_particleSize = 0;}}

		[SerializeField] private float m_ParticleSizeRandomMax = 4f;
		public float ParticleSizeRandomMax { get{return m_ParticleSizeRandomMax;} set{m_ParticleSizeRandomMax = value;} }

		[SerializeField] private float m_particleAspectRatio = 1f;
		public float ParticleAspectRatio { get {return m_particleAspectRatio;} set {m_particleAspectRatio = value;}}




		[SerializeField] private float m_ParticleSpeed = 0.2f;
		public float ParticleSpeed { get{return m_ParticleSpeed;} set{m_ParticleSpeed = value;} }
		

		[SerializeField] private float m_ParticleSpeedRandMin = 0;
		public float ParticleSpeedRandomMin { get{return m_ParticleSpeedRandMin;} set{m_ParticleSpeedRandMin = value;} }

		[SerializeField] private float m_ParticleSpeedRandMax = 0.2f;
		public float ParticleSpeedRandomMax { get{return m_ParticleSpeedRandMax;} set{m_ParticleSpeedRandMax = value;} }
		
		[SerializeField] private bool m_AlignedRotation = false;
		public bool AlignedRotation { get{return m_AlignedRotation;} set{m_AlignedRotation = value;} }

		[SerializeField] private bool m_Stretchable = false;
		public bool Stretchable { get{return m_Stretchable;} set{m_Stretchable = value;} }

		[RangeAttribute(1.0f, 100.0f)]
		[SerializeField] private float m_StretchableMultiplier = 1.0f;
		public float StretchableMultiplier { get{return m_StretchableMultiplier;} set{m_StretchableMultiplier = value;} }
		


		[SerializeField] private float m_ParticleAngle = 0;
		public float PartcileAngle { get{return m_ParticleAngle;} set{m_ParticleAngle = value;} }


		[SerializeField] private float m_ParticleAngleRandMin = 0;
		public float ParticleAngleRandomMin { get{return m_ParticleAngleRandMin;} set{m_ParticleAngleRandMin = value;} }

		[SerializeField] private float m_ParticleAngleRandMax = 0;
		public float ParticleAngleRandomMax { get{return m_ParticleAngleRandMax;} set{m_ParticleAngleRandMax = value;} }


		[SerializeField] private float m_ParticleSpinVel = 0;
		public float ParticleSpinVelocity { get{return m_ParticleSpinVel;} set{m_ParticleSpinVel = value;} }

		[SerializeField] private float m_ParticleSpinVelRandMin = 0;
		public float ParticleSpinVelocityRandomMin { get{return m_ParticleSpinVelRandMin;} set{m_ParticleSpinVelRandMin = value;} }

		[SerializeField] private float m_ParticleSpinVelRandMax = 0;
		public float ParticleSpinVelocityRandomMax { get{return m_ParticleSpinVelRandMax;} set{m_ParticleSpinVelRandMax = value;} }




		[SerializeField] private bool m_useLifeColor = false;
		public bool UseLifeColor { get{return m_useLifeColor;} set{m_useLifeColor = value;} }

		[SerializeField] private Gradient m_ParticleColorRamp = new Gradient();
		public Gradient ParticleColorRamp { get{return m_ParticleColorRamp;} set{m_ParticleColorRamp = value;} }

		[SerializeField] private Color m_ParticleColor = Color.black;
		public Color ParticleColor { get{return m_ParticleColor;} set{m_ParticleColor = value;} }
		
		[SerializeField] private Color[] m_ParticleColorArray;
		public Color[] ParticleColorsArray { get{return m_ParticleColorArray;} set{m_ParticleColorArray = value;} }
		
		[RangeAttribute(0,1)]
		[SerializeField] private float m_ParticleColorRandonR = 0.2f;
		public float ParticleColorRandomR { get{return m_ParticleColorRandonR;} set{m_ParticleColorRandonR = value;} }
		
		[RangeAttribute(0,1)]
		[SerializeField] private float m_ParticleColorRandonG = 0.2f;
		public float ParticleColorRandomG { get{return m_ParticleColorRandonG;} set{m_ParticleColorRandonG = value;} }
		
		[RangeAttribute(0,1)]
		[SerializeField] private float m_ParticleColorRandonB = 0.8f;
		public float ParticleColorRandomB { get{return m_ParticleColorRandonB;} set{m_ParticleColorRandonB = value;} }
		

		[RangeAttribute(0,1)]
		[SerializeField] private float m_ParticleAlpha = 1;
		public float ParticleAlpha { get{return m_ParticleAlpha;} set{m_ParticleAlpha = value;} }

		[SerializeField] private AnimationCurve m_particleAlphaCurve =  new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
		public AnimationCurve ParticleAlphaCurve { get {return m_particleAlphaCurve;} set {m_particleAlphaCurve = value;}}




		[SerializeField] private Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);
		public Rect uvRect { get{return m_UVRect;}
			set {if (m_UVRect == value) return;
				m_UVRect = value;
				SetVerticesDirty(); }
		}


		[SerializeField] private Texture[] m_TextureArray = null;
		public Texture[] TextureArray {
			get {
				return m_TextureArray == null ?  new Texture[]{mainTexture} : m_TextureArray; 
			} 
			set { if (m_TextureArray == value) return;
				m_TextureArray = value;
				SetVerticesDirty();
				SetMaterialDirty(); }
		}
		private List<Vector4> m_AtlasUVs;


		[SerializeField] private Texture m_Texture = null;
		public Texture texture { get {return m_Texture;} 
			set { if (m_Texture == value) return;
				m_Texture = value;
				SetVerticesDirty();
				SetMaterialDirty(); }
		}
		public override Texture mainTexture { 
			get { 
				if (material.mainTexture != null){
					return material.mainTexture;
				} else {
					if (m_TextureArray != null && m_TextureArray.Length > 1){
						return m_AtlasTexture;
					} else if ( m_TextureArray.Length == 1){
						return m_TextureArray[0];	
					} else {
						return m_Texture == null ? s_WhiteTexture : m_Texture;
					}
				}
			}
		}

		public override Material material
		{
			get
			{
				return (m_Material != null) ? m_Material : defaultMaterial;
			}
			set
			{
				if (m_Material == value)
					return;

				m_Material = value;
				SetMaterialDirty();
			}
		}


		[SerializeField] private bool m_isAnimated = false;
		public bool IsAnimated { get{return m_isAnimated;} set{m_isAnimated = value;} }

		[SerializeField] private bool m_isRandomStartFrame = false;
		public bool IsRandomStartFrame { get{return m_isRandomStartFrame;} set{m_isRandomStartFrame = value;} }
		private Texture m_AtlasTexture = null;
		private int m_GlobalParticleFrame = 0;
		private float m_GlobalParticleTime = 0;

		[SerializeField] private float m_ParticleFrameRate = 5;
		public float ParticleFrameRate { get{return m_ParticleFrameRate;} set{m_ParticleFrameRate = value;} }

		[SerializeField] private bool m_particleTextureAspectRatio;
		public bool ParticleTextureAspectRatio { get {return m_particleTextureAspectRatio;} set {m_particleTextureAspectRatio = value;}}

		
		private float m_TimeFromLastEmition = 0;
		private float m_TimeDelta = 0;
		private List<CanvasParticle> ParticleList = new List<CanvasParticle>();
		private List<CanvasParticle> ParticleListUnused = new List<CanvasParticle>();


		[SerializeField] private List<CanvasParticleCollider> m_ParticleColliders = new List<CanvasParticleCollider>();
		public List<CanvasParticleCollider> ParticleColliders { get { return m_ParticleColliders; } set { m_ParticleColliders = value; } }

		private Canvas m_canvas;
		private Vector2 m_canvasSize;
		private CanvasScaler m_canvasScaler;

		void OnDrawGizmos()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position + new Vector3(0, -5, 0), transform.position + new Vector3(0, 5, 0));
			Gizmos.DrawLine(transform.position + new Vector3(-5, 0, 0), transform.position + new Vector3(5, 0, 0));
		}


		#region PUBLIC EMITTER FUNCTIONS
		public void StartEmitter()
		{
			m_isActive = true;
			m_elapsedEmitterTime = 0;
		}

		public void PauseEmitter()
		{
			m_isActive = false;
		}

		public void FreezeEmitter()
		{
			m_isPlaying = false;
		}
		
		public void UnFreezeEmitter()
		{
			m_isPlaying = true;
		}
		
		public void StopEmitter()
		{
			m_isPlaying = true;
			DeleteAllParticles();
			m_isActive = false;
		}

		public void RestartEmitter()
		{
			m_isPlaying = true;
			StopEmitter();
			StartEmitter();
		}
		#endregion



		private void DeleteAllParticles()
		{
			ParticleList.Clear();
			m_numberOfParticles = 0;
		}

		protected override void Awake(){
			base.Awake();
			if (m_TextureArray != null && m_TextureArray.Length > 1){
				CreateTextureAtlas();
			}
		}

		protected override void Start(){
			//Debug.LogFormat("Screen res: {0} x {1}", Screen.width, Screen.height);
			//Debug.LogFormat("Canvas referencePixelsPerUnit: {0}", canvas.referencePixelsPerUnit);
			//Debug.LogFormat("Canvas scaleFactor: {0}", canvas.scaleFactor);
			//Debug.LogFormat("Canvas pixel rect: {0} x {1}", canvas.pixelRect.width, canvas.pixelRect.height);
			//Debug.LogFormat("Canvas size rect: {0} x {1}", (canvas.transform as RectTransform).sizeDelta.x, (canvas.transform as RectTransform).sizeDelta.y);
			//Debug.LogFormat("Canvas normalizedSortingGridSize: {0}", canvas.normalizedSortingGridSize);

			m_canvas = GetCanvas();
            m_canvasSize = (m_canvas.transform as RectTransform).sizeDelta;
            m_canvasScaler = m_canvas.gameObject.GetComponent<CanvasScaler>();

			//Debug.LogFormat("Canvas size: {0} x {1}", m_canvasSize.x, m_canvasSize.y);
			//Debug.LogFormat("Canvas sizes difference -> aspect Ax: {0}", (m_canvasSize.x / m_canvas.pixelRect.width));
			//Debug.LogFormat("Canvas sizes difference -> aspect Ay: {0}", (m_canvasSize.y / m_canvas.pixelRect.height));
			//Debug.LogFormat("Canvas sizes difference -> aspect Bx: {0}", (m_canvasScaler.referenceResolution.x / m_canvasSize.x));
			//Debug.LogFormat("Canvas sizes difference -> aspect By: {0}", (m_canvasScaler.referenceResolution.y / m_canvasSize.y));
			//Debug.LogFormat("Canvas sizes difference -> aspect Cx: {0}", (m_canvasScaler.referenceResolution.x / Screen.width));
			//Debug.LogFormat("Canvas sizes difference -> aspect Cy: {0}", (m_canvasScaler.referenceResolution.y / Screen.height));
			//Debug.LogFormat("Canvas sizes difference -> V: {0}", (m_canvasSize.x / m_canvas.pixelRect.width) * (m_canvasScaler.matchWidthOrHeight));
			//Debug.LogFormat("Canvas sizes difference -> H: {0}", (m_canvasSize.x / m_canvas.pixelRect.width) * (1- m_canvasScaler.matchWidthOrHeight));

            //Debug.LogFormat("Canvas sizes difference: {0}", (m_canvasSize.x / m_canvasSize.y));
			//Debug.LogFormat("Canvas sizes difference: {0}", (m_canvas.pixelRect.width / m_canvas.pixelRect.height));
			//Debug.LogFormat("Canvas sizes difference: {0}", (m_canvasSize.x / m_canvasSize.y) * m_canvas.scaleFactor);

			base.Start();
			m_objectPosition = transform.position;
			if (m_playOnAwake == true)
			{
				StartEmitter();
			}
		}

		protected override void OnEnable(){
			base.OnEnable();
			if (ParticleList == null)
			{
				ParticleList = new List<CanvasParticle>();
			}
			ParticleListUnused = new List<CanvasParticle>();

			if (m_TextureArray == null || m_TextureArray.Length == 0){
				m_TextureArray = new Texture[1];
			}

			if (m_EmitterShape == EmitterShapes.Image){
				m_EmitterImagePixels = CreateParticlePixels(m_EmitterShapeImage);
			}
		}

		protected override void OnDisable(){
			base.OnDisable();
			if (m_TextureArray != null && m_TextureArray.Length > 1){
				m_AtlasTexture = null;
			}
		}


		

		public void CreateTextureAtlas(){
			float TilesOffset = 2;
			List<int> validTextures = new List<int>();
			Vector2 TextureAtlasSize = new Vector2(TilesOffset, TilesOffset);
			m_AtlasUVs = new List<Vector4>();

			float greaterHeight = 0;
			for (int i = 0; i < m_TextureArray.Length; i++){
				if (m_TextureArray != null){
					if (m_TextureArray[i] != null){
						if (greaterHeight < m_TextureArray[i].height) greaterHeight = m_TextureArray[i].height + (TilesOffset * 2);
						TextureAtlasSize = new Vector2(TextureAtlasSize.x + m_TextureArray[i].width + TilesOffset, greaterHeight);
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
				Texture currentTexture = m_TextureArray[validTextures[i]];
				RenderTexture tempReadableTexture = RenderTexture.GetTemporary( currentTexture.width, currentTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
				Graphics.Blit(currentTexture, tempReadableTexture);
				RenderTexture.active = tempReadableTexture;
				newTextureAtlas.ReadPixels(new Rect(0, 0, tempReadableTexture.width, tempReadableTexture.height), currentXPosition, Mathf.RoundToInt(TilesOffset));
				newTextureAtlas.Apply();


				Vector4 newUV = new Vector4( (currentXPosition - TilesOffset/2)/newTextureAtlas.width, 0, (currentXPosition - TilesOffset/2 + m_TextureArray[i].width)/newTextureAtlas.width, 1);
				m_AtlasUVs.Add(newUV);

				RenderTexture.ReleaseTemporary(tempReadableTexture);
				RenderTexture.active = previous;
				currentXPosition += currentTexture.width + Mathf.RoundToInt(TilesOffset);
			}

			m_AtlasTexture = newTextureAtlas;
			SetVerticesDirty();
			SetMaterialDirty();
		}

		public void AddTexture(Texture texture){
			List<Texture> textureList = m_TextureArray.ToList();
			textureList.Add(texture);
			CreateTextureAtlas();
			m_TextureArray = textureList.ToArray();
		}

		public void RemoveTexture(int index){
			List<Texture> textureList = m_TextureArray.ToList();
			textureList.RemoveAt(index);
			CreateTextureAtlas();
			m_TextureArray = textureList.ToArray();
		}

		public void SwapTexture(int indexOld, int indexNew){
			Texture tempTexture = m_TextureArray[indexOld];
			m_TextureArray[indexOld] = m_TextureArray[indexNew];
			m_TextureArray[indexNew] = tempTexture;
			CreateTextureAtlas();
		}


		public List<PixelParticle> CreateParticlePixels(){
			return CreateParticlePixels(m_EmitterShapeImage);
		}
		public List<PixelParticle> CreateParticlePixels(Texture2D image){
			if (image == null)
				return null;

			for (int i = 0; i <  image.width; i++){
				for (int j = 0; j <  image.height ; j++){
					PixelParticle pixelParticle = new PixelParticle();
					pixelParticle.PixelColor = image.GetPixel(i, j);
					pixelParticle.PixelPosition = new Vector2(i, j);
					m_EmitterImagePixels.Add(pixelParticle);
				}
			}
			return m_EmitterImagePixels;
		}


		public virtual void OnBeforeSerialize()
		{

		}

		public virtual void OnAfterDeserialize()
		{
		}


		public void AddEmitterPoint(){
			AddEmitterPoint(Vector2.zero);
		}

		public void AddEmitterPoint(Vector2 point){
			m_EmitterShapePointListPoints.Add(point);
		}

		public void RemoveEmitterPoint(int index){
			m_EmitterShapePointListPoints.RemoveAt(index);
		}


		private void AddParticle()
		{
			int particlesToEmit = m_ParticlesPerEmission;
			if (m_MaxParticles > 0  &&  m_numberOfParticles + particlesToEmit > m_MaxParticles)
				particlesToEmit = m_MaxParticles - m_numberOfParticles;

			for (int i = 0; i < particlesToEmit; i++)
			{
				CanvasParticle newParticle;
				if (ParticleListUnused.Count == 0){
					newParticle = new CanvasParticle();
				} else {
					newParticle = ParticleListUnused[0];
					ParticleListUnused.RemoveAt(0);
				}
				float partSize = m_particleSize * m_particleSizeCurve.Evaluate(m_particleSizeCurve.keys[0].time);
				newParticle.ParticleVertices = new[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero};
				newParticle.ParticleTotalLife = m_ParticleLife + Random.Range(m_ParticleLifeRandMin, m_ParticleLifeRandMax);
				newParticle.ParticleLife = newParticle.ParticleTotalLife;
				newParticle.ParticleSpinVel = m_ParticleSpinVel + Random.Range(m_ParticleSpinVelRandMin, m_ParticleSpinVelRandMax);
				newParticle.ParicleRotation = m_ParticleAngle + Random.Range(m_ParticleAngleRandMin, m_ParticleAngleRandMax);
				newParticle.ParticleAlpha = m_ParticleAlpha * m_particleAlphaCurve.Evaluate(m_particleAlphaCurve.keys[0].time);
				if (m_useLifeColor == false){
					newParticle.ParticleOriginalColor = new Color(m_ParticleColor.r, m_ParticleColor.g, m_ParticleColor.b, newParticle.ParticleAlpha);
				} else {
					newParticle.ParticleOriginalColor = m_ParticleColorRamp.Evaluate(m_ParticleColorRamp.colorKeys[0].time);
				}
				if(m_ParticleColorArray != null && m_ParticleColorArray.Length > 0){
					newParticle.ParticleOriginalColor = m_ParticleColorArray[Random.Range(0,m_ParticleColorArray.Length)];
				}
				Color partColor = new Color(
					newParticle.ParticleOriginalColor.r + Random.Range(0, m_ParticleColorRandonR), 
					newParticle.ParticleOriginalColor.g + Random.Range(0, m_ParticleColorRandonG), 
					newParticle.ParticleOriginalColor.b + Random.Range(0, m_ParticleColorRandonB), 
					newParticle.ParticleAlpha);
				newParticle.ParticleColor = partColor;
				float partSpeed = (m_ParticleSpeed * 100f) + Random.Range(m_ParticleSpeedRandMin * 100f, m_ParticleSpeedRandMax * 100f);

				if (m_TextureArray.Length > 0){
					newParticle.ParticleTextureFrame = Random.Range(0, m_TextureArray.Length);
					if (m_isRandomStartFrame == false && m_isAnimated == true){
						newParticle.ParticleTextureFrame = m_GlobalParticleFrame;
					} 
				}

				newParticle.ParticleRandomSize = Random.Range(0, m_ParticleSizeRandomMax);
				newParticle.ParticleAspectRatio = m_particleAspectRatio;
				if (m_Texture != null && m_TextureArray.Length == 0){
					if (m_particleTextureAspectRatio == true)
						newParticle.ParticleAspectRatio = m_Texture.width / m_Texture.height;
				}
				if (m_TextureArray.Length > 0){
					if (m_particleTextureAspectRatio == true){
						if (m_TextureArray[newParticle.ParticleTextureFrame] != null){
							newParticle.ParticleAspectRatio = m_TextureArray[newParticle.ParticleTextureFrame].width / m_TextureArray[newParticle.ParticleTextureFrame].height;
						} 
					}
				}
				newParticle.ParticleSize = ((partSize + newParticle.ParticleRandomSize) * newParticle.ParticleAspectRatio) * m_particleSizeCurve.Evaluate(0);
				
				switch (m_EmitterShape)
				{
				case EmitterShapes.Directional:
					newParticle.ParticlePosition = Vector2.zero;
					newParticle.ParticleVelocity = new Vector2(Mathf.Sin( (Mathf.Deg2Rad * (m_EmitterDirection + Random.Range(- m_EmitterSpread/4, m_EmitterSpread/4)))), 
					                                           Mathf.Cos( (Mathf.Deg2Rad * (m_EmitterDirection + Random.Range(- m_EmitterSpread/4, m_EmitterSpread/4))))).normalized * partSpeed;
					break;
				case EmitterShapes.Point:
					newParticle.ParticlePosition = new Vector2(Random.Range(0,0), Random.Range(0,0));
					newParticle.ParticleVelocity = new Vector2(Mathf.Sin(Random.Range(0,360)), 
					                                           Mathf.Sin( Random.Range(0,100f))).normalized * partSpeed;
					break;
				case EmitterShapes.RectEmitter:

					Rect tempRect = m_EmitterShapeRect;
					if (m_EmitterShapeRectTransform != null){
						tempRect = GetRectCanvasSpace(m_EmitterShapeRectTransform);
					}
					switch (m_EmitterShapeRectType)
					{
					
					case EmitterShapeRectTypes.Area:
						newParticle.ParticlePosition = new Vector2(Random.Range(tempRect.xMin,tempRect.xMax), Random.Range(tempRect.yMin,tempRect.yMax));
						break;

					case EmitterShapeRectTypes.Edges:
						newParticle.ParticlePosition = Vector2.zero;
						switch (Random.Range(0,4))
						{
						case 0:
							newParticle.ParticlePosition = new Vector2(Random.Range(tempRect.xMin,tempRect.xMax), tempRect.yMin);
							break;
						case 1:
							newParticle.ParticlePosition = new Vector2(tempRect.xMin, Random.Range(tempRect.yMin,tempRect.yMax));
							break;
						case 2:
							newParticle.ParticlePosition = new Vector2(Random.Range(tempRect.xMin,tempRect.xMax), tempRect.yMax);
							break;
						case 3:
							newParticle.ParticlePosition = new Vector2(tempRect.xMax, Random.Range(tempRect.yMin,tempRect.yMax));
							break;
						default:
							newParticle.ParticlePosition = new Vector2(tempRect.xMax, Random.Range(tempRect.yMin,tempRect.yMax));
							break;
						}
						break;
					
					case EmitterShapeRectTypes.Vertices:
						Vector2[] rectVertices = new Vector2[] {new Vector2(tempRect.xMin, tempRect.yMin), new Vector2(tempRect.xMin, tempRect.yMax), new Vector2(tempRect.xMax, tempRect.yMax), new Vector2(tempRect.xMax, tempRect.yMin)};
						newParticle.ParticlePosition = rectVertices[Random.Range(0, rectVertices.Length)];
						break;
					}

					newParticle.ParticleVelocity = new Vector2(Mathf.Sin(Random.Range(0,360)), 
					                                           Mathf.Sin( Random.Range(0,100f))).normalized * partSpeed;

					break;
				case EmitterShapes.LineEmitter:
					newParticle.ParticlePosition = GetPointInSegment(m_EmitterShapeLinePointA, m_EmitterShapeLinePointB, Random.Range(0,100)/100.0f);
					newParticle.ParticleVelocity = new Vector2(Mathf.Sin( (Mathf.Deg2Rad * (m_EmitterDirection + Random.Range(- m_EmitterSpread/4, m_EmitterSpread/4)))), 
					                                           Mathf.Cos( (Mathf.Deg2Rad * (m_EmitterDirection + Random.Range(- m_EmitterSpread/4, m_EmitterSpread/4))))).normalized * partSpeed;
					break;
				case EmitterShapes.CircleEmitter:
					switch (m_EmitterShapeCircleType)
					{
					case EmitterShapeCircleTypes.Edge:
						int rndP1 = Random.Range(0, m_EmitterShapeCircleSegments);
						int rndP2 = rndP1+1;
						if (rndP2 >= m_EmitterShapeCircleSegments) rndP2=0;
						newParticle.ParticlePosition = GetPointInSegment(CircleShape(m_EmitterShapeCircleSegments, m_EmitterShapeCircleRadius)[rndP1], CircleShape(m_EmitterShapeCircleSegments, m_EmitterShapeCircleRadius)[rndP2], Random.Range(0,100)/100.0f);

						break;

					case EmitterShapeCircleTypes.Vertices:
						newParticle.ParticlePosition = CircleShape(m_EmitterShapeCircleSegments, m_EmitterShapeCircleRadius)[Random.Range(0, m_EmitterShapeCircleSegments)];
						break;
					}
					
					newParticle.ParticleVelocity = new Vector2(Mathf.Sin(Random.Range(0,360)), 
					                                           Mathf.Sin( Random.Range(0,100f))).normalized * partSpeed;

					break;
				case EmitterShapes.PointList:
					int randomPoint = Random.Range(0, m_EmitterShapePointListPoints.Count);
					newParticle.ParticlePosition = m_EmitterShapePointListPoints[randomPoint];//new Vector2(Random.Range(0,0), Random.Range(0,0));
					newParticle.ParticleVelocity = new Vector2(Mathf.Sin(Random.Range(0,360)), Mathf.Sin( Random.Range(0,100f))).normalized * partSpeed;
					break;

				case EmitterShapes.Image:
					if (m_EmitterImagePixels != null && m_EmitterImagePixels.Count > 1){
						int randomPixel = Random.Range(0, m_EmitterImagePixels.Count-1);
						newParticle.ParticlePosition = m_EmitterImagePixels[randomPixel].PixelPosition;//new Vector2(Random.Range(0,0), Random.Range(0,0));
						newParticle.ParticleColor = m_EmitterImagePixels[randomPixel].PixelColor;
						newParticle.ParticleVelocity = new Vector2(Mathf.Sin(Random.Range(0,360)), Mathf.Sin( Random.Range(0,100f))).normalized * partSpeed;
					}
					break;

				}
				newParticle.hasCollided = false;
				ParticleList.Add(newParticle);
			}
			m_numberOfParticles = ParticleList.Count;
		}

		private List<Vector2> CircleShape(int sides, float radius)
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


		private void RemoveParticle(CanvasParticle particle)
		{
			ParticleListUnused.Add(particle);
			ParticleList.Remove(particle);
			m_numberOfParticles = ParticleList.Count;
		}

		private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
		{
			Vector3 dir = point - pivot;
			dir = Quaternion.Euler(angles)*dir; 
			point = dir + pivot;
			return point; 
		}

		private Vector2 GetPointInSegment(Vector2 pointA, Vector2 pointB, float percent)
		{
			Vector2 newPoint;
			newPoint = new Vector2( pointA.x + (percent * (pointB.x - pointA.x)), pointA.y + (percent * (pointB.y - pointA.y)));
			return newPoint;
		}


		private Rect GetRectCanvasSpace(RectTransform rectTransform)
		{
			Vector3[] corners = new Vector3[4];
			Vector3[] screenCorners = new Vector3[2];
			//Canvas canvas = GetCanvas();
			rectTransform.GetWorldCorners(corners);

			if (m_canvas.renderMode == RenderMode.ScreenSpaceCamera || m_canvas.renderMode == RenderMode.WorldSpace)
			{
				screenCorners[0] = RectTransformUtility.WorldToScreenPoint(m_canvas.worldCamera, corners[1]);
				screenCorners[1] = RectTransformUtility.WorldToScreenPoint(m_canvas.worldCamera, corners[3]);
			}
			else
			{
				screenCorners[0] = RectTransformUtility.WorldToScreenPoint(null, corners[1]);
				screenCorners[1] = RectTransformUtility.WorldToScreenPoint(null, corners[3]);
			}
			float xMin = screenCorners[0].x - (m_canvas.GetComponent<RectTransform>().sizeDelta.x * rectTransform.pivot.x);
			float yMin = screenCorners[0].y - (m_canvas.GetComponent<RectTransform>().sizeDelta.y * rectTransform.pivot.y);
			float xMax = screenCorners[1].x - screenCorners[0].x;
			float yMax = screenCorners[1].y - screenCorners[0].y;
			return new Rect(xMin, yMin, xMax, yMax);
		}

		
		private Canvas GetCanvas()
		{
			GameObject selectedGo = this.gameObject;
			
			Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
			if (canvas != null && canvas.gameObject.activeInHierarchy) 
				return canvas;
			
			canvas = Object.FindObjectOfType(typeof(Canvas)) as Canvas;
			if (canvas != null && canvas.gameObject.activeInHierarchy)
				return canvas;

			return null;
		}

		void Update()
		{
			m_TimeDelta = Time.deltaTime;
			m_TimeFromLastEmition += m_TimeDelta;

            UpdateEmitter();
            RenderParticles();
            m_objectPosition = transform.position;
        }


		public void UpdateFromEditor()
		{
			if (m_isPlayingOnEditor == false)
				return;

			m_isActive = true;
			m_isPlaying = true;
#if UNITY_EDITOR
			if (m_lastEditorTimeSinceStartUp == 0)
			{
				m_lastEditorTimeSinceStartUp = (float)UnityEditor.EditorApplication.timeSinceStartup;
			}
			m_editorEmitterTime = (float)UnityEditor.EditorApplication.timeSinceStartup - m_lastEditorTimeSinceStartUp;
			m_lastEditorTimeSinceStartUp = (float)UnityEditor.EditorApplication.timeSinceStartup;
#endif
			m_TimeDelta = m_editorEmitterTime;

			m_TimeFromLastEmition += m_TimeDelta;
			UpdateEmitter();

			Canvas.ForceUpdateCanvases();
		}

		void UpdateEmitter()
		{
			//Debug.Log("UpdateEmitter");
			if (m_isPlaying == false)
				return;

			if (m_elapsedEmitterTime >= m_Duration && m_Duration > 0)
			{
				m_isActive = false;
			}

			if (m_TimeFromLastEmition >= 1.0f / m_EmitterRate && m_isActive == true)
			{
				m_TimeFromLastEmition = 0;
				m_elapsedEmitterTime += m_TimeDelta;
				AddParticle();
			}

			if (Application.isPlaying && m_isActive == false && m_AutoKill == true && ParticleList.Count == 0 && m_Duration > 0)
			{
				GameObject.Destroy(gameObject);
			}

            SetVerticesDirty();
            SetMaterialDirty();
        }

		Vector2 waveForce;
		Vector2 turbulenceForce;
		Vector2 gravityForce;
		Vector2 totalForce;
		Vector2 oldPosition;
		public override void Rebuild(CanvasUpdate update)
		{
			//Debug.Log("Canvas rebuild");
			base.Rebuild(update);
            //RenderParticles();
        }


        public void RenderParticles() { 
			//Debug.Log("Render particles");
			for (int i = 0; i < ParticleList.Count; i++)
			{
				waveForce 		= new Vector2( 
				                                  Mathf.Sin(Time.time * m_Wave.x) * (m_WaveAmplitude.x/1000.0f) , 
				                                  Mathf.Sin(Time.time * m_Wave.y) * (m_WaveAmplitude.y/1000.0f) );
				turbulenceForce = new Vector2( 
				                                      (Mathf.PerlinNoise(Time.time * m_Turbulence.x, 0) * (m_TurbulenceAmplitude.x / 100.0f)) - m_TurbulenceAmplitude.x/200.0f , 
				                                      (Mathf.PerlinNoise(Time.time * m_Turbulence.y, 0) * (m_TurbulenceAmplitude.y / 100.0f)) - m_TurbulenceAmplitude.y/200.0f  );
				gravityForce 	= m_Gravity/10.0f;
				totalForce 		= gravityForce + waveForce + turbulenceForce;
				//totalForce 		= gravityForce + waveForce;
				oldPosition 	= ParticleList[i].ParticlePosition;
                if (m_worldSpace == true && m_canvasScaler != null)
                {
					if (m_canvasScaler.screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
					{
						ParticleList[i].ParticlePosition -= (Vector2)((transform.position - m_objectPosition) * (((m_canvasScaler.referenceResolution.x / Screen.width) * (1 - m_canvasScaler.matchWidthOrHeight)) + ((m_canvasScaler.referenceResolution.y / Screen.height) * m_canvasScaler.matchWidthOrHeight)));
					} else
                    {
                        ParticleList[i].ParticlePosition -= (Vector2)((transform.position - m_objectPosition) * m_canvasScaler.scaleFactor);
					}
                }
				ParticleList[i].ParticleVelocity += totalForce;
				ParticleList[i].ParticlePosition += (ParticleList[i].ParticleVelocity * m_TimeDelta);
				ParticleList[i].ParticleLife -= m_TimeDelta; 
				ParticleList[i].ParicleRotation += ParticleList[i].ParticleSpinVel; 

				if (m_AlignedRotation == true){
					float MovingAngle = Mathf.Atan2(ParticleList[i].ParticlePosition.y-oldPosition.y, ParticleList[i].ParticlePosition.x-oldPosition.x)*180 / Mathf.PI;
					ParticleList[i].ParicleRotation = MovingAngle;

					if (m_Stretchable == true){
						ParticleList[i].ParticleAspectRatio = m_particleAspectRatio * ParticleList[i].ParticleVelocity.magnitude * (m_StretchableMultiplier / 100f);
					}
				}

				float particleLifeNormalized = ParticleList[i].ParticleLife / ParticleList[i].ParticleTotalLife;
				ParticleList[i].ParticleSize = (m_particleSize + ParticleList[i].ParticleRandomSize) * m_particleSizeCurve.Evaluate((1.0f - particleLifeNormalized) * m_particleSizeCurve.keys[m_particleSizeCurve.length-1].time);
				ParticleList[i].ParticleAlpha = m_ParticleAlpha * m_particleAlphaCurve.Evaluate((1.0f - particleLifeNormalized) * m_particleAlphaCurve.keys[m_particleAlphaCurve.length-1].time);

				if (m_useLifeColor == false){
					ParticleList[i].ParticleColor = new Color(ParticleList[i].ParticleColor.r, ParticleList[i].ParticleColor.g, ParticleList[i].ParticleColor.b, ParticleList[i].ParticleAlpha );
				} else {
					Color newParticleColor = m_ParticleColorRamp.Evaluate((1.0f - particleLifeNormalized) * m_ParticleColorRamp.colorKeys[m_ParticleColorRamp.colorKeys.Length-1].time);
					ParticleList[i].ParticleColor = new Color(newParticleColor.r, newParticleColor.g, newParticleColor.b, ParticleList[i].ParticleAlpha);
				}
				if (ParticleList[i].ParticleLife <= 0 && m_ParticleLife != 0)
					RemoveParticle(ParticleList[i]);
			
				//collision check
				if (m_ParticleColliders.Count > 0){
					//foreach (CanvasParticleCollider particleCollider in m_ParticleColliders){
					CanvasParticleCollider particleCollider = m_ParticleColliders[0];
						if (particleCollider.ColliderBoxRect.Contains(ParticleList[i].ParticlePosition)){
							//if (ParticleList[i].hasCollided == false){
							//	ParticleList[i].hasCollided = true;
						ParticleList[i].ParticleVelocity =  (ParticleList[i].ParticleVelocity - (ParticleList[i].ParticleVelocity * particleCollider.Friction));// * particleCollider.Bounciness);
							//}
						}
					//}
				}
			}
            //m_objectPosition = transform.position;
        }

  //      private void FixedUpdate()
  //      {
  //          RenderParticles();
		//	m_objectPosition = transform.position;
		//	Canvas.ForceUpdateCanvases();
		//}

		// Unity 4.6 to 5.1 rendering methods
#if UNITY_4_6 || UNITY_5_0 || UNITY_5_1
	protected void SetVbo(List<UIVertex> vbo, Vector2[] vertices, Vector2[] uvs, Color partColor)
	{
		for (int i = 0; i < vertices.Length; i++)
		{
			var vert = UIVertex.simpleVert;
			vert.color = partColor;
			vert.position = vertices[i];
			vert.uv0 = uvs[i];
			vbo.Add(vert);
		}
	}

	protected override void OnFillVBO (List<UIVertex> vbo)
	{
			vbo.Clear();
			for (int i = 0; i < ParticleList.Count; i++)
			{
			CanvasParticle particle = new CanvasParticle();
			particle = ParticleList[i];

				Vector2 part = particle.ParticlePosition;
				Vector2 partCorner1 =  part + new Vector2( -(particle.ParticleSize / 2) * (particle.ParticleAspectRatio/1), -(particle.ParticleSize / 2));
				Vector2 partCorner2 =  part + new Vector2( (particle.ParticleSize / 2) * (particle.ParticleAspectRatio/1), (particle.ParticleSize / 2));
				
				particle.ParticleVertices[0] = new Vector2( partCorner1.x, partCorner1.y);
				particle.ParticleVertices[1] = new Vector2( partCorner1.x, partCorner2.y);
				particle.ParticleVertices[2] = new Vector2( partCorner2.x, partCorner2.y);
				particle.ParticleVertices[3] = new Vector2( partCorner2.x, partCorner1.y);

				particle.ParticleVertices[0] = RotatePointAroundPivot(particle.ParticleVertices[0], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));
				particle.ParticleVertices[1] = RotatePointAroundPivot(particle.ParticleVertices[1], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));
				particle.ParticleVertices[2] = RotatePointAroundPivot(particle.ParticleVertices[2], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));
				particle.ParticleVertices[3] = RotatePointAroundPivot(particle.ParticleVertices[3], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));

				Vector2[] uvs;
				if (m_TextureArray.Length > 1){
					Vector4 textUV;
					if (m_isAnimated == true){
						particle.ParticleAnimTime++;
						m_GlobalParticleTime++;
						
						if (m_isRandomStartFrame == false){
							if (m_GlobalParticleTime >= (30 * ParticleList.Count) /m_ParticleFrameRate){
								m_GlobalParticleFrame++;
								if(m_GlobalParticleFrame >= m_TextureArray.Length){
									m_GlobalParticleFrame = 0;
								}
								m_GlobalParticleTime = 0;
							}	
							particle.ParticleTextureFrame = m_GlobalParticleFrame;
						} else {
							if (particle.ParticleAnimTime >= 30/m_ParticleFrameRate){
								particle.ParticleTextureFrame++;
								if(particle.ParticleTextureFrame >= m_TextureArray.Length){
									particle.ParticleTextureFrame = 0;
								}
								particle.ParticleAnimTime = 0;
							}
						}
						
					}
					if (particle.ParticleTextureFrame >= m_AtlasUVs.Count){
					particle.ParticleTextureFrame = m_AtlasUVs.Count-1;
					}
					textUV = AtlasUVs[particle.ParticleTextureFrame];
					Vector2 uvTopLeft = new Vector2(textUV.x, textUV.y);
					Vector2 uvBottomLeft = new Vector2(textUV.x, textUV.w);
					Vector2 uvTopRight = new Vector2(textUV.z, textUV.y);
					Vector2 uvBottomRight = new Vector2(textUV.z, textUV.w);
					uvs = new[] { uvBottomLeft, uvTopLeft, uvTopRight, uvBottomRight };
				} else {
					Vector2 uvTopLeft = Vector2.zero;
					Vector2 uvBottomLeft = new Vector2(0, 1);
					Vector2 uvTopRight = new Vector2(1, 0);
					Vector2 uvBottomRight = new Vector2(1, 1);
					uvs = new[] { uvBottomLeft, uvTopLeft, uvTopRight, uvBottomRight };
				}
				SetVbo(vbo, particle.ParticleVertices, uvs, particle.ParticleColor);
			}
	}
#endif


#if UNITY_5_3
		[System.Obsolete("Obsolete on Unity version greater than 5.3.1. Use the OnPopulateMesh(VertexHelper vh) instead.")]
		protected override void OnPopulateMesh(Mesh mesh)
		{
			mesh.Clear(false);
			VertexHelper vh = new VertexHelper (mesh);
			OnPopulateMesh (vh);
			vh.FillMesh (mesh);
		}
#endif
#if UNITY_5_3 || UNITY_5_4_OR_NEWER
		//unity 5.3 rendering methods
		protected void SetMesh(VertexHelper vh , Vector2[] vertices, Vector2[] uvs, Color partColor)
		{
			int[] verticesIndex = new int[vertices.Length];
			
			for (int i = 0; i < vertices.Length; i++)
			{
				var vert = UIVertex.simpleVert;
				vert.color = partColor;
				vert.position = vertices[i];
				vert.uv0 = uvs[i];
				vh.AddVert(vert);

				verticesIndex[i] = vh.currentVertCount -1;
			}

			vh.AddTriangle(verticesIndex[0], verticesIndex[1], verticesIndex[2]);
			vh.AddTriangle(verticesIndex[2], verticesIndex[3], verticesIndex[0]);
		}
		
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			for (int i = 0; i < ParticleList.Count; i++)
			{
				CanvasParticle particle = new CanvasParticle();
				particle = ParticleList[i];
				
				Vector2 part = particle.ParticlePosition;
				Vector2 partCorner1 =  part + new Vector2( -(particle.ParticleSize / 2) * (particle.ParticleAspectRatio/1), -(particle.ParticleSize / 2));
				Vector2 partCorner2 =  part + new Vector2( (particle.ParticleSize / 2) * (particle.ParticleAspectRatio/1), (particle.ParticleSize / 2));

				particle.ParticleVertices[0] = new Vector2(partCorner1.x, partCorner1.y);
				particle.ParticleVertices[1] = new Vector2(partCorner1.x, partCorner2.y);
				particle.ParticleVertices[2] = new Vector2(partCorner2.x, partCorner2.y);
				particle.ParticleVertices[3] = new Vector2(partCorner2.x, partCorner1.y);
				
				particle.ParticleVertices[0] = RotatePointAroundPivot(particle.ParticleVertices[0], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));
				particle.ParticleVertices[1] = RotatePointAroundPivot(particle.ParticleVertices[1], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));
				particle.ParticleVertices[2] = RotatePointAroundPivot(particle.ParticleVertices[2], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));
				particle.ParticleVertices[3] = RotatePointAroundPivot(particle.ParticleVertices[3], particle.ParticlePosition, new Vector3(0, 0, particle.ParicleRotation));
				
				Vector2[] uvs;
				if (m_TextureArray.Length > 1 ){
					Vector4 textUV;
					if (m_isAnimated == true){
						particle.ParticleAnimTime++;
						m_GlobalParticleTime++;

						if (m_isRandomStartFrame == false){
							if (m_GlobalParticleTime >= (30 * ParticleList.Count) /m_ParticleFrameRate){
								m_GlobalParticleFrame++;
								if(m_GlobalParticleFrame >= m_TextureArray.Length){
									m_GlobalParticleFrame = 0;
								}
								m_GlobalParticleTime = 0;
							}	
							particle.ParticleTextureFrame = m_GlobalParticleFrame;
						} else {
							if (particle.ParticleAnimTime >= 30/m_ParticleFrameRate){
								particle.ParticleTextureFrame++;
								if(particle.ParticleTextureFrame >= m_TextureArray.Length){
									particle.ParticleTextureFrame = 0;
								}
								particle.ParticleAnimTime = 0;
							}
						}

					}
					if (particle.ParticleTextureFrame >= m_AtlasUVs.Count){
						particle.ParticleTextureFrame = m_AtlasUVs.Count-1;
					}
					textUV = m_AtlasUVs[particle.ParticleTextureFrame];
					Vector2 uvTopLeft = new Vector2(textUV.x, textUV.y);
					Vector2 uvBottomLeft = new Vector2(textUV.x, textUV.w);
					Vector2 uvTopRight = new Vector2(textUV.z, textUV.y);
					Vector2 uvBottomRight = new Vector2(textUV.z, textUV.w);
					uvs = new[] { uvBottomLeft, uvTopLeft, uvTopRight, uvBottomRight };
				} else {
					Vector2 uvTopLeft = Vector2.zero;
					Vector2 uvBottomLeft = new Vector2(0, 1);
					Vector2 uvTopRight = new Vector2(1, 0);
					Vector2 uvBottomRight = new Vector2(1, 1);
					uvs = new[] { uvBottomLeft, uvTopLeft, uvTopRight, uvBottomRight };
				}
				SetMesh(vh, particle.ParticleVertices, uvs, particle.ParticleColor);
			}
		}
#endif

}
}
