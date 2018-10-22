using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlock : MonoBehaviour {

	public PlayerController player;

	public bool dashUnlocked = false;
	public bool crouchUnlocked = false;


	public int dashIndex = 0;
	public int[] dashCost = {10, 0};
	[TextArea] public string[] dashLabel = {
		"Faster\nCooldown",
		"Sold\nOut"
	};

	public int crouchIndex = 0;
	public int[] crouchCost = {10, 0};
	[TextArea] public string[] crouchLabel = {
		"Unlock\nAbility",
		"Sold\nOut"
	};

	public int genIndex = 0;
	public int[] genCost = {10, 0};
	[TextArea] public string[] genLabel = {
		"Increase\nHealth",
		"Sold\nOut"
	};

	public int dashCheat, crouchCheat, genCheat;

	void Start() {
		player = GetComponent<PlayerController>();
		for (int i = 0; i < dashCheat; i++) {
			UpgradeDash();
		}
		for (int i = 0; i < crouchCheat; i++) {
			UpgradeCrouch();
		}
		for (int i = 0; i < genCheat; i++) {
			UpgradeGen();
		}
	}

	public void UpgradeDash() {
		dashUnlocked = true;

		switch(dashIndex) {
			case 0:
				player.dashesAllowed++;
				break;
			case 1:
				player.dashChargeTime = 3f;
				break;
			case 2:
				player.dashesAllowed++;
				break;
			case 3:
				player.dashChargeTime = 1.5f;
				break;
			case 4:
				player.dashesAllowed++;
				break;
			case 5:
				player.dashChargeTime = .75f;
				break;
			case 6:
				player.dashesAllowed++;
				break;
		}

		if (dashIndex < dashCost.Length - 1) dashIndex++;
	}

	public void UpgradeCrouch() {
		

		switch(crouchIndex) {
			case 0:
				crouchUnlocked = true;
				break;
			case 1:
				player.crouchJumpSpeed = 10;
				break;
			case 2:
				player.crouchJumpSpeed = 12;
				break;
			case 3:
				player.crouchJumpSpeed = 15;
				break;
		}
		if (crouchIndex < crouchCost.Length - 1) crouchIndex++;
	}

	public void UpgradeGen() {

		switch(genIndex) {
			case 0:
				player.maxHealth++;
				break;
			case 1:
				player.xSpeed = 4;
				break;
			case 2:
				player.maxHealth++;
				break;
			case 3:
				player.jumpSpeed = 8;
				break;
			case 4:
				player.maxHealth++;
				break;
			case 5:
				player.xSpeed = 6;
				break;
			case 6:
				player.maxHealth++;
				break;
		}
		player.health = player.maxHealth;
		CanvasController.instance.UpdateHealth(player.health);

		if (genIndex < genCost.Length - 1 ) genIndex++;
	}


}
