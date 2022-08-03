using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ParticleOnTouch : MonoBehaviour {

	[SerializeField] private CanvasParticleEmitter m_particleEmitter;

	void Update () {
		if (Input.GetMouseButtonDown(0) ) {
			GameObject.Instantiate(m_particleEmitter.gameObject, Input.mousePosition, Quaternion.identity, transform);
		}

	}
}
