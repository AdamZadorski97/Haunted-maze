using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ParticleSystem))]
public class ShurikenCanvasRenderer : MaskableGraphic {

	[SerializeField] private float m_sizeMultiplier = 1f;

	private ParticleSystem m_particleSystem;
	private ParticleSystemRenderer m_particleRenderer;
	private ParticleSystem.Particle[] m_particleArray = null;

	private bool isReady = false;

	private List<Vector4> m_AtlasUVs;

	private Texture m_Texture = null;
	public Texture texture { get {return m_Material.mainTexture;} 
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
				return m_Texture == null ? s_WhiteTexture : m_Texture;
			}
		}
	}

	private Texture m_AtlasTexture = null;

	public override Material material
	{ get {
			if (GetComponent<ParticleSystemRenderer>().sharedMaterial != null){
				m_Material = GetComponent<ParticleSystemRenderer>().sharedMaterial;
				return GetComponent<ParticleSystemRenderer>().sharedMaterial;
			} else {
				return defaultMaterial;
			}
		}
	}




	private new void Awake(){
		m_particleSystem = GetComponent<ParticleSystem>();
		if (m_particleSystem != null ){
			if (m_particleArray != null)
			{
				isReady = true;
				m_particleSystem.GetParticles(m_particleArray);
			}
		} else {
			isReady = false;
			Debug.LogError("A ParticleSystem component attached to this object is required to render Shuriken Particles into Canvas.");
		}

		if (m_particleSystem.textureSheetAnimation.enabled == true){
			SetParticlesUVs();
		}
	}

	new void Start () {
		if (!isReady){
			return;
		}
		if (m_particleSystem.main.playOnAwake == true){
			m_particleSystem.Play();
		}
		if (m_particleRenderer == null){
			m_particleRenderer = transform.GetComponent<ParticleSystemRenderer>();
		}
	}


	protected override void OnEnable(){
		base.OnEnable();
		if (m_particleRenderer == null){
			m_particleRenderer = transform.GetComponent<ParticleSystemRenderer>();
		}

		if (m_particleRenderer.sharedMaterial != null){
			m_Material = m_particleRenderer.sharedMaterial;
		}
		SetParticlesUVs();
		SetVerticesDirty();
		SetMaterialDirty();
	}

	protected override void OnDisable(){
		base.OnDisable();
	}


	private void SetParticlesUVs(){
		if (mainTexture == null) {
			return;
		}

		m_AtlasUVs = new List<Vector4>();
		Texture texture = mainTexture;
		Vector2 UVSize = Vector2.zero;
		bool isFlipU = System.Convert.ToBoolean(m_particleSystem.textureSheetAnimation.flipU);
		bool isFlipV = System.Convert.ToBoolean(m_particleSystem.textureSheetAnimation.flipV);

		switch (m_particleSystem.textureSheetAnimation.mode) {
		case ParticleSystemAnimationMode.Sprites:
			for (int i = 0; i < m_particleSystem.textureSheetAnimation.spriteCount; i++) {
				Sprite sprite = m_particleSystem.textureSheetAnimation.GetSprite(i);
				float U1 = sprite.textureRect.xMin;
				float U2 = sprite.textureRect.xMax;
				float V1 = sprite.textureRect.yMax;
				float V2 = sprite.textureRect.yMin;
				Vector4 newUV = new Vector4(
					(isFlipU == false) ? U1 : U2,
					(isFlipV == false) ? V1 : V2,
					(isFlipU == false) ? U2 : U1,
					(isFlipV == false) ? V2 : V1
				);
				m_AtlasUVs.Add(newUV);
			}
			break;
		default:
			int divisionsX = m_particleSystem.textureSheetAnimation.numTilesX;
			int divisionsY = 0;
			int offsetY = 0;

			switch (m_particleSystem.textureSheetAnimation.animation) {
			case ParticleSystemAnimationType.WholeSheet:
				divisionsY = m_particleSystem.textureSheetAnimation.numTilesY;
				break;
			case ParticleSystemAnimationType.SingleRow:
				divisionsY = 1;
				if (m_particleSystem.textureSheetAnimation.useRandomRow == true){
					offsetY = Random.Range(1, m_particleSystem.textureSheetAnimation.numTilesY);
				} else {
					offsetY = m_particleSystem.textureSheetAnimation.rowIndex;
				}
				break;
			default:
				divisionsX = 1;
				divisionsY = 1;
				break;
			}

			UVSize = new Vector2(texture.width / m_particleSystem.textureSheetAnimation.numTilesX, texture.height / m_particleSystem.textureSheetAnimation.numTilesY);
			for (int i = offsetY; i < (offsetY + divisionsY); i++){
				for (int j = 0; j < divisionsX; j++){
					float U1 = (UVSize.x * j) / texture.width;
					float U2 = ((UVSize.x * j) + UVSize.x) / texture.width;
					float V1 = ((UVSize.y * (i+offsetY)) + UVSize.y)/ texture.height;
					float V2 = (UVSize.y * (i+offsetY)) / texture.height;
					Vector4 newUV = new Vector4(
						(isFlipU == false) ? U1 : U2,
						(isFlipV == false) ? V1 : V2,
						(isFlipU == false) ? U2 : U1,
						(isFlipV == false) ? V2 : V1
					);
					m_AtlasUVs.Add(newUV);
				}
			}
			break;
		}
	}



	private void Update()
	{
		if (m_particleArray == null || m_particleArray.Length < m_particleSystem.main.maxParticles){
			m_particleArray = new ParticleSystem.Particle[m_particleSystem.main.maxParticles];
		}
		UpdateEmitter();
	}



	void UpdateEmitter(){
		SetVerticesDirty();
		SetMaterialDirty();
	}

	public override void Rebuild (CanvasUpdate update)
	{
		base.Rebuild (update);
	}

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

		int numberOfParticles = m_particleSystem.GetParticles(m_particleArray);

		if (m_particleArray == null || m_particleArray.Length <= 0){
			return;
		}

		for (int i = 0; i < numberOfParticles; i++)
		{
			ParticleSystem.Particle particle = m_particleArray[i];
			Vector2[] particleVertices = new[] { Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero};
			float size = particle.GetCurrentSize(m_particleSystem) * m_sizeMultiplier;
			Vector3 partPosition = particle.position * m_sizeMultiplier;
			Vector2 partCorner1 =  (Vector2)(partPosition) + new Vector2( -(size / 2), -(size / 2));
			Vector2 partCorner2 =  (Vector2)(partPosition) + new Vector2( (size / 2), (size / 2));

			particleVertices[0] = new Vector2(partCorner1.x, partCorner1.y);
			particleVertices[1] = new Vector2(partCorner1.x, partCorner2.y);
			particleVertices[2] = new Vector2(partCorner2.x, partCorner2.y);
			particleVertices[3] = new Vector2(partCorner2.x, partCorner1.y);

			particleVertices[0] = CanvasParticleHelper.RotatePointAroundPivot(particleVertices[0], partPosition, new Vector3(0,0, -particle.rotation));
			particleVertices[1] = CanvasParticleHelper.RotatePointAroundPivot(particleVertices[1], partPosition, new Vector3(0,0, -particle.rotation));
			particleVertices[2] = CanvasParticleHelper.RotatePointAroundPivot(particleVertices[2], partPosition, new Vector3(0,0, -particle.rotation));
			particleVertices[3] = CanvasParticleHelper.RotatePointAroundPivot(particleVertices[3], partPosition, new Vector3(0,0, -particle.rotation));

			Vector2[] particleUV = new Vector2[4];
			if (m_AtlasUVs != null && m_AtlasUVs.Count > 1){
				int particleFrame = Mathf.FloorToInt( (((particle.startLifetime  - particle.remainingLifetime)) * m_AtlasUVs.Count) / particle.startLifetime);

				Vector4 textUV = m_AtlasUVs[particleFrame];
				particleUV[0] = new Vector2(textUV.x, textUV.w);
				particleUV[1] = new Vector2(textUV.x, textUV.y);
				particleUV[2] = new Vector2(textUV.z, textUV.y);
				particleUV[3] = new Vector2(textUV.z, textUV.w);
			} else {
				particleUV[0] = new Vector2(0, 1);
				particleUV[1] = Vector2.zero;
				particleUV[2] = new Vector2(1, 0);
				particleUV[3] = new Vector2(1, 1);
			}


			SetMesh(vh, particleVertices, particleUV, particle.GetCurrentColor(m_particleSystem));
		}
	}



}
