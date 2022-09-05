using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestParticleEmiter : MonoBehaviour {

	public GameObject[] ParticleDemos;

	public Transform m_container;
	public Button m_NextButton;
	public Button m_PreviousButton;
	private int m_currentIndex;
	private GameObject m_currentParticle;

	public Button m_StartEmitterButton;
	public Button m_PauseEmitterButton;
	public Button m_RestartEmitterButton;
	public Button m_StopEmitterButton;
	public Button m_FreezeEmitterButton;
	public Button m_UnfreezeEmitterButton;



	// Use this for initialization
	void Start () {
		m_currentIndex = 0;

		m_NextButton.onClick.AddListener(NextDemoParticle);
		m_PreviousButton.onClick.AddListener(PreviousDemoParticle);

		CreateDemoParticle();
	}

	void NextDemoParticle(){
		m_currentIndex++;
		if (m_currentIndex > ParticleDemos.Length -1)
			m_currentIndex = 0;

		CreateDemoParticle();
	}

	void PreviousDemoParticle(){
		m_currentIndex--;
		if (m_currentIndex < 0)
			m_currentIndex = ParticleDemos.Length -1;
		CreateDemoParticle();
	}


	void CreateDemoParticle()
	{
		m_StartEmitterButton.onClick.RemoveAllListeners();
		m_PauseEmitterButton.onClick.RemoveAllListeners();
		m_StopEmitterButton.onClick.RemoveAllListeners();
		m_RestartEmitterButton.onClick.RemoveAllListeners();
		m_FreezeEmitterButton.onClick.RemoveAllListeners();
		m_UnfreezeEmitterButton.onClick.RemoveAllListeners();

		GameObject.Destroy(m_currentParticle);
		m_currentParticle = GameObject.Instantiate(ParticleDemos[m_currentIndex]) as GameObject;
		m_currentParticle.transform.SetParent(m_container, false);
		CanvasParticleEmitter[] particleEmitters = m_currentParticle.GetComponentsInChildren<CanvasParticleEmitter>();

		foreach (CanvasParticleEmitter emitter in particleEmitters){
			m_StartEmitterButton.onClick.AddListener(emitter.StartEmitter);
			m_PauseEmitterButton.onClick.AddListener(emitter.PauseEmitter);
			m_StopEmitterButton.onClick.AddListener(emitter.StopEmitter);
			m_RestartEmitterButton.onClick.AddListener(emitter.RestartEmitter);
			m_FreezeEmitterButton.onClick.AddListener(emitter.FreezeEmitter);
			m_UnfreezeEmitterButton.onClick.AddListener(emitter.UnFreezeEmitter);
		}

	}


}
