using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Base member vars
	private Rigidbody2D rb2d;
	private float maxSpeed = 5;
	private int health = 3;
	private Quaternion rotation;
	

	// Speed pickup vars
	private float speedBoostAmount = 5;
	private float speedBoostTimer = 0;
	private bool spedUp = false;


	//Dmg pickup vars
	private float dmgBoostAmount = 2;
	private float dmgBoostTimer = 0;
	private bool dmgUp = false;

	// Weapon vars
	public float damage = 1;
	public bool hasGun = false;
	private int bulletCounter = 0;
	public bool hasSword = false;
	private int swordCounter = 0;
	private bool attacking = false;
	private float attackTimer = 0;
	
	public GameObject bullet;
	private GameObject[] BulletPool;

	// Main logic 
	// ==================================

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		BulletPool = new GameObject[20];
		CreateBulletPool();
	}

	void CreateBulletPool() {
		for (int i = 0; i < BulletPool.Length; i++) {
			BulletPool[i] = Instantiate(bullet, new Vector3(-200, -200, 0), Quaternion.identity) as GameObject;
			BulletPool[i].GetComponent<Bullet>().SetActive(false);
		}
	}

	void FixedUpdate() {
		
		// Movement and Attacking
		float hInput = Input.GetAxis ("Horizontal");
		float vInput = Input.GetAxis ("Vertical");
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		bool attack = Input.GetMouseButtonDown(0);
		if(attack) { Attack(); }

		// Deactivate Speed boost after timer
		if (spedUp && (Time.time - speedBoostTimer > 10)) {
			maxSpeed -= speedBoostAmount;
			spedUp = false;
		}

		// Deactivate dmg boost after timer
		if (dmgUp && (Time.time - dmgBoostTimer > 10)) {
			damage /= dmgBoostAmount;
			dmgUp = false;
		}

		//Deactivate attacking hitbox after attacking animation runs
		if (Time.time - attackTimer > 1 /* seconds */) {
			attacking = false;
		}

		//Rotation to mousePos
		rb2d.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x))*Mathf.Rad2Deg - 90);
		rb2d.velocity = new Vector2 (hInput * maxSpeed, vInput * maxSpeed);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (attacking) {
			if (other.gameObject.tag == "Enemy") {
				// Deal damage
			}
		}
	}


	// Attacking functions
	// ==================================
	
	void Attack() {
		if (hasGun) {
			if (bulletCounter > 0) {
			Shoot();
			}
		} else if (hasSword) {
			// if (swordCounter > 0) {
			// 	Slash();
			// }
		} else {
			//Attack animation for fists
		}

		//Drop weapon when counter runs out
		if (bulletCounter <= 0) {
			hasGun = false;
			GetComponent<Animator>().SetBool("hasGun", false);
		}
		if (swordCounter <= 0) {
			hasSword = false;
			GetComponent<Animator>().SetBool("hasSword", false);			
		}

	}


	void Shoot() {
		bulletCounter--;
		//GetComponent<Animator>().SetTrigger("shooting");		
		for (int i = 0; i < BulletPool.Length; i++) {
			if (!BulletPool[i].GetComponent<Bullet>().GetActive()) {
				BulletPool[i].GetComponent<Bullet>().Spawn();
				return;
			}
		}
	}

	void Slash() {
		attacking = true;
		attackTimer = Time.time;
		// GetComponent<Animator>().SetTrigger("slashing");
	}


	// Getters / Setters + Pickups
	// ==================================
	

	public Quaternion GetRotation() {
		return rotation;
	}

	public void AddHealth(int amount) {
		health += amount;
	}

	public void LoseHealth(int amount) {
		health -= amount;
	}

	public int GetHealth() {
		return health;
	}

	public void SpeedBoost() {
		spedUp = true;
		speedBoostTimer = Time.time;
		maxSpeed += speedBoostAmount;
	}

	public void DamageBoost() {
		dmgUp = true;
		dmgBoostTimer = Time.time;
		damage *= dmgBoostAmount;
	}

	public void PickupGun() {
		hasGun = true;
		bulletCounter += 20;
		GetComponent<Animator>().SetBool("hasGun", true);
	}

	public void PickupSword() {
		hasSword = true;
		swordCounter += 20;
		GetComponent<Animator>().SetBool("hasSword", true);
	}
}
