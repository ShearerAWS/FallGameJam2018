using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : MonoBehaviour {

	public float speed = 2;
	public Transform target;
	public Vector2 dir;
	public float rotateRate;
	public GameObject deathEffect;

	public int money = 1;
	public float targetDist = 4f;

	public float chargeTime = .6f;
	public float chargeTimer;

	public float dashTime = .4f;
	public float dashTimer;

	public float dashSpeed = 6;

	public float rechargeTime = .4f;
	public float rechargeTimer;

	public GameObject dashEffect;
	public bool spawnedEffect = false;
	public bool startedDash;


	void Start() {
		target = PlayerController.instance.transform;
		dir = (target.position - transform.position).normalized;
	}
	// Update is called once per frame
	void Update () {
		float dist = (target.position - transform.position).magnitude;
		if (dist >targetDist && !startedDash) {
			dir = (target.position - transform.position).normalized;
			transform.rotation = Quaternion.FromToRotation(Vector3.up, (Vector3) dir);
			transform.position += (Vector3)(dir * speed * Time.deltaTime);
			chargeTimer = 0f;
		} else {
			startedDash = true;
			if (chargeTimer < chargeTime) {
				chargeTimer += Time.deltaTime;
				dashTimer = 0f;
			} else {
				if (dashTimer < dashTime) {
					if (!spawnedEffect) {
						spawnedEffect = true;
						Instantiate(dashEffect, transform.position, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.left, dir) - 45f), transform);
					}
					transform.position += (Vector3)(dir * dashSpeed * Time.deltaTime);
					dashTimer += Time.deltaTime;
					rechargeTimer = 0f;
				} else {
					if (rechargeTimer < rechargeTime) {
						Vector2 dirTarget = (target.position - transform.position).normalized;
						
						Quaternion rotate = Quaternion.FromToRotation(Vector3.up, (Vector3) dir);
						Quaternion rotate2 = Quaternion.FromToRotation(Vector3.up, (Vector3) dirTarget);

						rotate = Quaternion.Lerp(rotate, rotate2, rechargeTimer / rechargeTime);

						transform.rotation = rotate;
						rechargeTimer += Time.deltaTime;
					} else {
						dashTimer = 0f;
						chargeTimer = 0f;
						rechargeTimer = 0f;
						spawnedEffect = false;
						startedDash = false;
						dir = (target.position - transform.position).normalized;
					}
					
				}
				

			}
			
		}
		
	

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
