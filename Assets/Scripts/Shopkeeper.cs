using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class Shopkeeper : MonoBehaviour {

	public TextMeshPro text;
	private Animator anim;

	public List<string> messages;

	public CinemachineVirtualCamera shopCam;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			text.text = messages[Random.Range(0, messages.Count)];
			anim.SetTrigger("TextShow");
			shopCam.m_Priority = 20;
			CanvasController.instance.HideUI();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			shopCam.m_Priority = 0;
			CanvasController.instance.ShowUI();

		}
	}
	

	void OnCollisionEnter2D(Collision2D collision) {
		if (!CanvasController.instance.inShop)
			CanvasController.instance.ShowShop();
	}
}
