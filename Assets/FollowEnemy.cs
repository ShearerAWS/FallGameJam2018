using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : MonoBehaviour {

	public float speed = 2;
	public Transform target;
	public Vector2 dir;
	public float rotateRate;
	public GameObject deathEffect;

	public int money = 1;

	void Start() {
		target = PlayerController.instance.transform;
		dir = (target.position - transform.position).normalized;
	}
	// Update is called once per frame
	void Update () {
		Vector2 targetDir = (target.position - transform.position).normalized;
		dir = Vector2.Lerp(dir, targetDir, rotateRate * Time.deltaTime).normalized;
	
		transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3) dir);
		transform.position += (Vector3)(dir * speed * Time.deltaTime);
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == "Player") {
			if (PlayerController.instance.state == PlayerController.State.dash) {
				Instantiate(deathEffect, transform.position, Quaternion.identity);
				Destroy(gameObject);
				PlayerController.instance.IncreaseMoney(money);
				
			} else {
				if (PlayerController.instance.canHurt)
					PlayerController.instance.Damage();
			}
		}
	}
}
