using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	private int health;
	public int maxHealth = 2;

	void Start () {
		health = maxHealth;
		CanvasController.instance.UpdateHealth(health);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
