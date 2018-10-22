using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ArenaStartTrigger : MonoBehaviour {

	public GameObject enterDoor;
	public static ArenaStartTrigger instance;

	public GameObject followEnemy, dashEnemy, fastFollowEnemy;
	public bool started = false;

	public float timer;
	public float spawnTime = 7f;
	public int arenaProgress = 0;

	public Transform[] spawns;

	void Awake() {
		instance = this;
	}
	
	void Update() {
		if (started) {
			timer += Time.deltaTime;
			if (timer > spawnTime) {
				timer = 0;
				Spawn();
				arenaProgress++;
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			if (!started) {
				timer = 0;
				arenaProgress = 0;
				started = true;
				enterDoor.SetActive(true);
				Spawn();
				Spawn();
			}
		}
	}

	private void Spawn() {
		Vector3 pos = spawns[GetSpawnAwayFromPlayer()].position + (2*new Vector3(Random.Range(-10,10),Random.Range(-10,10),0).normalized);
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, (Vector3) (PlayerController.instance.transform.position - pos));
		if (arenaProgress < 4) {
			Instantiate(followEnemy, pos, rot);
		} else if (arenaProgress < 8) {
			if (Random.Range(0,2) == 0) {
				Instantiate(followEnemy, pos, rot);
			} else {
				Instantiate(dashEnemy, pos, rot);
			}
			if (arenaProgress == 6) spawnTime = 5;
		} else if (arenaProgress < 12) {
			if (Random.Range(0,4) == 0) {
				Instantiate(followEnemy, pos, rot);
			} else {
				Instantiate(dashEnemy, pos, rot);
			}
			if (arenaProgress == 10) spawnTime = 4;
		} else if (arenaProgress < 16) {
			if (Random.Range(0,3) == 0) {
				if (Random.Range(0,2) == 0) {
					Instantiate(followEnemy, pos, rot);
				} else {
					Instantiate(fastFollowEnemy, pos, rot);
				}
			} else {
				Instantiate(dashEnemy, pos, rot);
			}
			if (arenaProgress == 14) spawnTime = 3;
		} else {
			if (Random.Range(0,3) == 0) {
				if (Random.Range(0,4) == 0) {
					Instantiate(followEnemy, pos, rot);
				} else {
					Instantiate(fastFollowEnemy, pos, rot);
				}
			} else {
				Instantiate(dashEnemy, pos, rot);
			}
			if (arenaProgress == 25) spawnTime = 2f;
		}
	}

	private int GetSpawnAwayFromPlayer() {
		int closestIndex = -1;
		float dist = 10000f;
		for(int i = 0; i < spawns.Length; i++) {
			float spawnDist = (PlayerController.instance.transform.position - spawns[i].position).magnitude;
			if (spawnDist < dist) {
				dist = spawnDist;
				closestIndex = i;
			} 
		}
		int val = -1;
		while (val == -1 || val == closestIndex) {
			val = Random.Range(0, spawns.Length); 
		}
		return val;
	}
}
