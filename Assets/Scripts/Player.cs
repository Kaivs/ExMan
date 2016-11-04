using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Base member vars
	private Transform m_gunPos;
	private Animator m_anim;
	private Rigidbody2D rb2d;
	public float maxSpeed = 10;
	public bool takingDamage = false;
	private Quaternion rotation;

	// Restrict camera movement
	public GameObject m_camera;
	private float vCamExtent;
	private float hCamExtent;
	private Vector3 restrictedPos;	

	// Speed pickup vars
	private float speedBoostAmount = 5;
	private float speedBoostTimer = 0;
	private bool spedUp = false;


	//Dmg pickup vars
	private int dmgBoostAmount = 2;
	private float dmgBoostTimer = 0;
	private bool dmgUp = false;

	// Weapon vars
	public int damage = 1;
	public bool hasGun = false;
	public int bulletCounter = 0;
	public bool hasSword = false;
	public int swordCounter = 0;
	private bool attacking = false;
	private float attackTimer = 0;
	
	public GameObject bullet;
	private GameObject[] BulletPool;
	public int health = 12;	
	public int Weapon;

	// Main logic 
	// ==================================

	void Start () {
		rb2d = GetComponent<Rigidbody2D>();

		vCamExtent = m_camera.GetComponent<Collider2D>().bounds.extents.y;
		hCamExtent = m_camera.GetComponent<Collider2D>().bounds.extents.x;
		restrictedPos = transform.position;
		BulletPool = GameManager.Instance.CreateObjectPool("bullet_player", GameManager.Instance.PlayerBulletPoolCount);

		m_anim = GetComponent<Animator>();
		m_gunPos = transform.GetChild(0).GetComponent<Transform>();
	}

	void Update() {		
		
		// Movement and Attacking
		float hInput = Input.GetAxis ("Horizontal");
		float vInput = Input.GetAxis ("Vertical");

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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


		m_anim.SetBool("isAttacking", false);
		
		bool attack = Input.GetMouseButtonDown(0);
		if(attack) { Attack(); }
		
		if (rb2d.velocity != Vector2.zero) {
			m_anim.SetBool("isMoving", true);
		}
		else {
			m_anim.SetBool("isMoving", false);			
		}
		

		//Rotation to mousePos
		rb2d.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x))*Mathf.Rad2Deg - 90);

		// Movement
		rb2d.velocity = new Vector2 (hInput * maxSpeed, vInput * maxSpeed);
	}

	void LateUpdate()
	{		
	}

	void FixedUpdate() {
		RestrictMovement();		
	}

	void OnTriggerStay2D(Collider2D other) {
		if (attacking) {
			if (other.gameObject.tag == "Enemy") {
				other.gameObject.GetComponent<EnemyAI>().LoseHealth(damage);
				// Deal damage to enemy
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (attacking) {
			if (other.gameObject.tag == "Enemy") {
				other.gameObject.GetComponent<EnemyAI>().LoseHealth(damage);
				// Deal damage to enemy
			}
		}
	}

	//Restrict player movement to on Camera
	void RestrictMovement() {
		restrictedPos = transform.position;
		if (transform.position.x > (m_camera.transform.position.x + hCamExtent)) {
			restrictedPos.x = m_camera.transform.position.x + hCamExtent;
		} else if (transform.position.x < (m_camera.transform.position.x - hCamExtent)){
			restrictedPos.x = m_camera.transform.position.x - hCamExtent;
		}
		if (transform.position.y > (m_camera.transform.position.y + vCamExtent)) {
			restrictedPos.y = m_camera.transform.position.y + vCamExtent;
		} else if (transform.position.y < (m_camera.transform.position.y - vCamExtent)){
			restrictedPos.y = m_camera.transform.position.y - vCamExtent;
		}

		transform.position = restrictedPos;
		
	}


	// Attacking functions
	// ==================================
	
	void Attack() {
		m_anim.SetBool("isAttacking", true);

		if (hasGun) {
			if (bulletCounter > 0) {
			Shoot();
			}
		} else if (hasSword) {
			if (swordCounter > 0) {
				Slash();
			}
		} else {
			FistAttack();
		}

		//Drop weapon when counter runs out
		if (bulletCounter <= 0) {
			hasGun = false;
			m_anim.SetBool("hasGun", false);
		}
		if (swordCounter <= 0) {
			hasSword = false;
			m_anim.SetBool("hasSword", false);			
		}

		if (!hasSword && !hasGun) {	
			Weapon = 0;
		}

	}

	void FistAttack() {
		attacking = true;
		attackTimer = Time.time;
	}



	void Shoot() {
		bulletCounter--;
		//GetComponent<Animator>().SetTrigger("shooting");		
		for (int i = 0; i < BulletPool.Length; i++) {
			if (!BulletPool[i].GetComponent<Bullet>().GetActive()) {
				BulletPool[i].GetComponent<Bullet>().Spawn(m_gunPos.position, Input.mousePosition, Bullet.Ownership.Player, damage, true);
				return;
			}
		}
	}

	void Slash() {
		swordCounter--;
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
		health += amount;
	}

	public void LoseHealth(int amount) {
		health -= amount;
	}

	public int GetHealth() {
		return health < 0 ? 0 : health;
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
		if (hasGun) {
			bulletCounter += 20;
		} else {
			bulletCounter = 20;
		}
		hasGun = true;
		hasSword = false;
		Weapon = 1;
		m_anim.SetBool("hasGun", true);
		m_anim.SetBool("hasSword", false);
		
	}

	public void PickupSword() {
		if (hasSword) {
			swordCounter += 20;
		} else {
			swordCounter = 20;
		}
		hasSword = true;
		hasGun = false;
		Weapon = 2;
		m_anim.SetBool("hasSword", true);
		m_anim.SetBool("hasGun", false);
		
	}
}
