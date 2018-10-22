using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCamera : MonoBehaviour {

	public static MainCamera instance;
	private CinemachineBrain brain;
	
	void Awake() {
		instance = this;
		brain = GetComponent<CinemachineBrain>();
	}
	
	public void ApplyShake(float timeout) {
		StartCoroutine(shake(timeout));
	}

	IEnumerator shake(float time) {
		// 
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("vcam")) {
			CinemachineVirtualCamera vcam = g.GetComponent<CinemachineVirtualCamera>();
			vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = .4f;
		}
		yield return new WaitForSeconds(time);
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("vcam")) {
			CinemachineVirtualCamera vcam = g.GetComponent<CinemachineVirtualCamera>();
			vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
		}
	}
}
