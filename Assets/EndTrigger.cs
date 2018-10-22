using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour {

	private bool search;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (search) {
			float dist = transform.position.y - PlayerController.instance.transform.position.y;
			dist = 23f - dist;
			dist /= 20;
			
			dist = Mathf.Clamp(dist, 0,1);
			CanvasController.instance.setEndAlpha(dist);

			if (PlayerController.instance.transform.position.y > transform.position.y) {
				SceneManager.LoadScene("Scenes/endScene");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			search = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			search = false;
			CanvasController.instance.setEndAlpha(0);
		}
	}
}
