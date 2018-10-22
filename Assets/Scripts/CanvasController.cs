using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour {

	public static CanvasController instance;

	public GameObject dashCanvas, moneyCanvas, healthCanvas, shopCanvas;
	public CanvasGroup endCanvas;
	
	public Slider dashCharge;
	public TextMeshProUGUI dashCount;

	public List<Image> healthBars;

	public TextMeshProUGUI moneyCount, shopDashLabel, shopCrouchLabel, shopGenLabel;
	public TextMeshProUGUI dashMoney, crouchMoney, genMoney;

	public GameObject genPoint, crouchPoint, dashPoint, exitPoint;
	public enum ShopSelect {dash, crouch, gen, exit};
	public ShopSelect select;
	public bool inShop;
	public bool canInteractShop = false;
	public Transform postShopWarp;

	private Animator anim;
	

	// Use this for initialization
	void Awake () {
		instance = this;
		anim = GetComponent<Animator>();
	}

	void Start() {
		UpdateMoney(PlayerController.instance.money);
	}
	
	// Update is called once per frame
	void Update () {
		if (canInteractShop) {
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
				switch (select) {
					case ShopSelect.dash:
						select = ShopSelect.crouch;
						break;
					case ShopSelect.crouch:
						select = ShopSelect.gen;
						break;
					case ShopSelect.gen:
						select = ShopSelect.dash;
						break;
				}
			} 

			if (Input.GetKeyDown(KeyCode.RightArrow)  || Input.GetKeyDown(KeyCode.D)) {
				switch (select) {
					case ShopSelect.dash:
						select = ShopSelect.gen;
						break;
					case ShopSelect.crouch:
						select = ShopSelect.dash;
						break;
					case ShopSelect.gen:
						select = ShopSelect.crouch;
						break;
				}
			} 

			if (Input.GetKeyDown(KeyCode.UpArrow)  || Input.GetKeyDown(KeyCode.W)) {
				if (select == ShopSelect.exit) {
					select = ShopSelect.crouch;
				}
			} 

			if (Input.GetKeyDown(KeyCode.DownArrow)  || Input.GetKeyDown(KeyCode.S)) {
				select = ShopSelect.exit;
			} 
			UpdateShopPointer();

			if (Input.GetKeyDown(KeyCode.Space)) {
				PlayerController p = PlayerController.instance;
				switch(select) {
					case ShopSelect.dash:
						if (p.money >= p.unlock.dashCost[p.unlock.dashIndex]) {
							p.money -= p.unlock.dashCost[p.unlock.dashIndex];
							p.unlock.UpgradeDash();
						}
						break;
					case ShopSelect.crouch:
						if (p.money >= p.unlock.crouchCost[p.unlock.crouchIndex]) {
							p.money -= p.unlock.crouchCost[p.unlock.crouchIndex];
							p.unlock.UpgradeCrouch();
						}
						break;
					case ShopSelect.gen:
						if (p.money >= p.unlock.genCost[p.unlock.genIndex]) {
							p.money -= p.unlock.genCost[p.unlock.genIndex];
							p.unlock.UpgradeGen();
						}
						break;
					case ShopSelect.exit:
						inShop = false;
						HideShop();
						break;
				}
				UpdateShop();
				UpdateMoney(PlayerController.instance.money);
			}
		}
	}

	public void UpdateDashUI(float value, int count) {
		dashCharge.value = value;
		dashCount.text = count.ToString();
		if (PlayerController.instance.dashesAllowed == 1) {
			dashCount.enabled = false;
		} else {
			dashCount.enabled = true;
		}
	}

	public void UpdateHealth(int health) {
		for(int i = 0; i < healthBars.Count; i++) {
			healthBars[i].enabled = i < health;
		}
	}

	public void UpdateMoney(int money) {
		string m = money.ToString();
		if (money < 10) {
			m = "0" + m;
		}
		moneyCount.text = m;
	}

	public void HideUI() {
		moneyCanvas.SetActive(false);
		dashCanvas.SetActive(false);
		healthCanvas.SetActive(false);
	}

	public void ShowUI() {
		moneyCanvas.SetActive(true);
		dashCanvas.SetActive(true);
		healthCanvas.SetActive(true);
	}

	public void setEndAlpha(float a) {
		endCanvas.alpha = a;
	}

	public void ShowShop() {
		anim.SetTrigger("showShop");
		
		UpdateShop();
		inShop = true;
		select = ShopSelect.exit;
		UpdateShopPointer();

		StartCoroutine(WaitThenWarp());
	}
	IEnumerator WaitThenWarp() {
		yield return new WaitForSeconds(1f);
		PlayerController.instance.inShop = true;
		canInteractShop = true;
	}

	public void HideShop() {
		PlayerController.instance.inShop = false;
		canInteractShop = false;
		inShop = false;
		HideUI();
		anim.SetTrigger("hideShop");
	}

	public void UpdateShop() {
		PlayerUnlock p = PlayerController.instance.unlock;
		shopDashLabel.text = p.dashLabel[p.dashIndex];
		shopCrouchLabel.text = p.crouchLabel[p.crouchIndex];
		shopGenLabel.text = p.genLabel[p.genIndex];

		dashMoney.text = p.dashCost[p.dashIndex].ToString();
		crouchMoney.text = p.crouchCost[p.crouchIndex].ToString();
		genMoney.text = p.genCost[p.genIndex].ToString();
	}

	public void UpdateShopPointer() {
		dashPoint.SetActive(select == ShopSelect.dash);
		crouchPoint.SetActive(select == ShopSelect.crouch);
		genPoint.SetActive(select == ShopSelect.gen);
		exitPoint.SetActive(select == ShopSelect.exit);
	}

	public void FadeOut() {
		anim.SetTrigger("fadeOut");
	}
	public void PostFade() {
		PlayerController.instance.MoveBack();
	}
}
