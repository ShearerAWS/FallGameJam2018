
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance;

	public PlayerUnlock unlock;

	public int maxHealth;
	public int health;

	public float xSpeed = 3f;
	public float jumpSpeed = 5f;
	public float dashSpeed = 5f;
	public float crouchJumpSpeed = 10f;

	public Transform groundCheck1, groundCheck2;
	public Transform wallCheck;
	public LayerMask groundLayer;
	[HideInInspector] public bool grounded;
	[HideInInspector] public bool hitWall;

	public int dashesAllowed = 5;
	private int dashesLeft = 1;
	private Vector2 dashDirection;
	public float dashTime = 1f;
	private float dashTimer;
	public GameObject dashEffect;

	private float dashChargeTimer;
	public float dashChargeTime = 10f;

	private bool crouch;
	public float crouchChargeTime = 2f;
	private float crouchChargeTimer;
	public GameObject crouchEffect;

	//private float wallJumpTimer = .3f;
	//public GameObject wallJumpTime;

	private Rigidbody2D rb;
	private Animator anim;

	public enum State {move, dash};
	public State state = State.move;

	public int money;
	public bool inShop = false;

	public bool canHurt = true;
	public GameObject deathEffect;
	public Transform startLocation;
	public GameObject door;



	// Use this for initialization
	void Awake() {
		instance = this;
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		unlock = GetComponent<PlayerUnlock>();
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (inShop) {
			transform.position = CanvasController.instance.postShopWarp.position;
			rb.velocity = Vector2.zero;
			return;
		}

		// input
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");
		float xAxisRaw = Input.GetAxisRaw("Horizontal");
		float yAxisRaw = Input.GetAxisRaw("Vertical");
		bool jumpPress = Input.GetKeyDown(KeyCode.Space);

		// speed
		float xVel = xAxis * xSpeed;
		float yVel = rb.velocity.y;
		
		// detection
		grounded = Physics2D.OverlapCircle(groundCheck1.position, .1f, groundLayer) || 
				   Physics2D.OverlapCircle(groundCheck2.position, .1f, groundLayer);
		hitWall = Physics2D.OverlapCircle(wallCheck.position, .1f, groundLayer);

		// crouch charge
		crouch = unlock.crouchUnlocked && yAxisRaw < 0 && grounded && xAxisRaw == 0;
		if (crouch) {
			crouchChargeTimer += Time.deltaTime;
		} else {
			if (crouchChargeTimer > crouchChargeTime && xVel == 0f) {
				yVel = crouchJumpSpeed;
				Instantiate(crouchEffect, transform.position, Quaternion.Euler(0,0,225), transform);
				MainCamera.instance.ApplyShake(.2f);
			}
			crouchChargeTimer = 0;
		}

		// dash charge && grounded?
		if (grounded && dashesAllowed != dashesLeft) {
			if (crouch) {
				dashChargeTimer += 1.7f * Time.deltaTime;
			} else {
				dashChargeTimer += Time.deltaTime;
			}
		}
		if (dashChargeTimer > dashChargeTime) {
			if (dashesLeft < dashesAllowed) {
				dashesLeft++;
				dashChargeTimer = 0f;	
			}
		}
		float dashValue = Mathf.Clamp(dashChargeTimer / dashChargeTime, 0, 1);
		if (dashesAllowed == dashesLeft) {
			dashValue = 1f;
		}
		if (unlock.dashUnlocked) {
			CanvasController.instance.UpdateDashUI(dashValue, dashesLeft);
		}

		// dash timer
		if (state == State.dash) {
			dashTimer += Time.deltaTime;
			if (dashTimer > dashTime) {
				state = State.move;
				StartCoroutine(IFrames2());
				yVel *= .5f;
			}
		}

		// jump
		if (jumpPress) {
			if (grounded) {
				yVel = jumpSpeed;
			} else if (unlock.dashUnlocked && dashesLeft > 0 && state != State.dash) {
				dashesLeft--;
				dashDirection = new Vector2(xAxisRaw, yAxisRaw).normalized;
				if (dashDirection == Vector2.zero) {
					//yVel = jumpSpeed;
				} else {
					anim.SetTrigger("dash");
					state = State.dash;
					Instantiate(dashEffect, transform.position, Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.left, dashDirection) - 45f), transform);
					dashTimer = 0f;
					MainCamera.instance.ApplyShake(dashTime * .5f);
				}
			}
		}

		// set velocity
		if (state == State.move) {
			rb.velocity = new Vector2(xVel, yVel);
		} if (state == State.dash) {
			rb.velocity = dashDirection * dashSpeed;
		}

		// animation
		anim.SetBool("walking", xAxisRaw != 0);
		anim.SetBool("grounded", grounded);
		anim.SetBool("crouch", crouch);
	}

	public void IncreaseMoney(int m) {
		money += m;
		CanvasController.instance.UpdateMoney(money);
	}

	public void Damage() {
		health--;
		if (health == 0) {
			canHurt = false;
			Restart();
		} else {
			CanvasController.instance.UpdateHealth(health);
			StartCoroutine(IFrames());
			MainCamera.instance.ApplyShake(.5f);
		}
		
	}

	IEnumerator IFrames() {
		canHurt = false;
		yield return new WaitForSeconds(.10f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(.10f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(.15f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(.10f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(.15f);
		GetComponent<SpriteRenderer>().enabled = false;
		yield return new WaitForSeconds(.10f);
		GetComponent<SpriteRenderer>().enabled = true;
		yield return new WaitForSeconds(.15f);
		canHurt = true;
	}

	IEnumerator IFrames2() {
		canHurt = false;
		yield return new WaitForSeconds(.3f);
		canHurt = true;
	}

	public void Restart() {
		CanvasController.instance.UpdateHealth(health);
		GetComponent<SpriteRenderer>().enabled = false;
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		MainCamera.instance.ApplyShake(2f);
		rb.velocity = Vector2.zero;
		StartCoroutine(WaitToFade());

		
	}

	IEnumerator WaitToFade() {
		yield return new WaitForSeconds(1f);
		CanvasController.instance.FadeOut();
	}

	public void MoveBack() {
		health = maxHealth;
		GetComponent<SpriteRenderer>().enabled = true;
		CanvasController.instance.UpdateHealth(health);
		transform.position = startLocation.position;
		canHurt = true;
		door.SetActive(false);
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) {
			Destroy(g);
		}
		ArenaStartTrigger.instance.started = false;
		dashesLeft = dashesAllowed;
		dashChargeTimer = 0f;
	}
}
