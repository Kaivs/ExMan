using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

	// Base member vars
	private Rigidbody2D rb2d;
	public float maxSpeed = 10;
	public bool takingDamage = false;
	private Quaternion rotation;

	//Audio Files
	private AudioSource m_audioManager;
	public AudioClip GunShot;
	public AudioClip GunCock;
	public AudioClip SwordSwing;
	public AudioClip SwordDraw;
	public AudioClip HealthPickUp;
	public AudioClip SpeedPickUp;

	

	// Speed pickup vars
	public float speedBoostAmount = 5;
	public float speedBoostTimer = 0;
	public bool spedUp = false;
	public int pickupSpeed = 0;


	//Dmg pickup vars
	private float dmgBoostAmount = 2;
	private float dmgBoostTimer = 0;
	private bool dmgUp = false;

	// Weapon vars
	public float damage = 1;
	public bool hasGun = false;
	public int bulletCounter = 0;
	public bool hasSword = false;
	public int swordCounter = 0;
	public bool attacking = false;
	private float attackTimer = 0;
	
	public GameObject bullet;
	private GameObject[] BulletPool;
	public int health = 12;	
	public int Weapon;

	// Main logic 
	// ==================================

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		BulletPool = new GameObject[20];
		CreateBulletPool();
		m_audioManager = GetComponent<AudioSource>();
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
			pickupSpeed = 0;
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
				// Deal damage to enemy
			}
		}
	}


	// Attacking functions
	// ==================================
	
	void Attack() {
		if (hasGun) {
			if (bulletCounter > 0) {
			Shoot();
			m_audioManager.PlayOneShot(GunShot, .5f);
			
			}
		} else if (hasSword) {
			if (swordCounter > 0) {
				Slash();
				m_audioManager.PlayOneShot(SwordSwing, .5f);
			}
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

		if (!hasSword && !hasGun) {	
			Weapon = 0;
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
		swordCounter--	;
		attacking = true;
		attackTimer = Time.time;
		// GetComponent<Animator>().SetTrigger("slashing");
	}

	// Getters / Setters + Pickups
	// ====================================
	
	void TakeDamage(){

	}

	public Quaternion GetRotation() {
		return rotation;
	}

	public void AddHealth(int amount) {
		m_audioManager.PlayOneShot(HealthPickUp, .5f);
		health += amount;
	}

	public void LoseHealth(int amount) {
		health -= amount;
	}

	public int GetHealth() {
		return health;
	}

	public void SpeedBoost() {	
		m_audioManager.PlayOneShot(SpeedPickUp, .5f);
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
		m_audioManager.PlayOneShot(GunCock, .5f);		
		if (hasGun) {
			bulletCounter += 20;
		} else {
			bulletCounter = 20;
		}
		hasGun = true;
		hasSword = false;
		Weapon = 1;
		GetComponent<Animator>().SetBool("hasGun", true);
		GetComponent<Animator>().SetBool("hasSword", false);
		
	}

	public void PickupSword() {
		m_audioManager.PlayOneShot(SwordDraw, .5f);
		if (hasSword) {
			swordCounter += 20;
		} else {
			swordCounter = 20;
		}
		hasSword = true;
		hasGun = false;
		Weapon = 2;
		GetComponent<Animator>().SetBool("hasSword", true);
		GetComponent<Animator>().SetBool("hasGun", false);
		
	}
}
