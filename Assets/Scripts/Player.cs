using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	// Audio Files
	private AudioSource m_audioManager;
	public AudioClip GunShot;
	public AudioClip GunCock;
	public AudioClip SwordSwing;
	public AudioClip SwordDraw;
	public AudioClip HealthPickUp;
	public AudioClip SpeedPickUp;
	public bool m_isDead;


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

	public float deadCoolDown = 2f;


	// Main logic 
	// ==================================

	void Start () {
		m_isDead = false;
		rb2d = GetComponent<Rigidbody2D>();

		vCamExtent = m_camera.GetComponent<Collider2D>().bounds.extents.y;
		hCamExtent = m_camera.GetComponent<Collider2D>().bounds.extents.x;
		restrictedPos = transform.position;
		BulletPool = GameManager.Instance.CreateObjectPool("bullet_player", GameManager.Instance.PlayerBulletPoolCount);

		m_anim = GetComponent<Animator>();
		m_gunPos = transform.GetChild(0).GetComponent<Transform>();

		m_audioManager = GetComponent<AudioSource>();
	}

	void Update() {		
		if (!m_isDead) {
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
		else {
			deadCoolDown -= Time.deltaTime;
			if (deadCoolDown <= 0) {
				GameManager.Instance.GameOver();
				SceneManager.LoadScene("GameOver");
			}
		}
	}

	void FixedUpdate() {
		if (!m_isDead) {
			RestrictMovement();		
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (attacking) {
			if (other.gameObject.tag == "Enemy") {
				other.gameObject.GetComponent<EnemyAI>().LoseHealth(Damage());
				// Deal damage to enemy
				attacking = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (attacking) {
			if (other.gameObject.tag == "Enemy") {
				other.gameObject.GetComponent<EnemyAI>().LoseHealth(Damage());
				// Deal damage to enemy
				attacking = false;
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

	int Damage() {
		if (hasSword) {
			return 2;
		}
		else {
			return damage;
		}
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
		bool bulletFly = false;
		for (int i = 0; i < BulletPool.Length && !bulletFly; i++) {
			if (!BulletPool[i].GetComponent<Bullet>().GetActive()) {
				BulletPool[i].GetComponent<Bullet>().Spawn(m_gunPos.position, Input.mousePosition, Bullet.Ownership.Player, Damage(), true);
				bulletFly = true;
			}
		}
		m_audioManager.PlayOneShot(GunShot, .5f);
	}

	void Slash() {
		swordCounter--;
		attacking = true;
		attackTimer = Time.time;
		// GetComponent<Animator>().SetTrigger("slashing");
		m_audioManager.PlayOneShot(SwordSwing, .75f);
	}

	// Getters / Setters + Pickups
	// ====================================

	public int isSpedUp() {
		if (spedUp) {
			return 1;
		} else {
			return 0;
		}
	}

	public Quaternion GetRotation() {
		return rotation;
	}

	public void AddHealth(int amount) {
		health += amount;
		if (health > 10) {
			health = 10;
		}
	}

	public void LoseHealth(int amount) {
		if (!m_isDead) {
			health -= amount;		
			if (health <= 0) {
				Die();
			}
		}
	}

	public void Die() {
		m_isDead = true;
		GetComponent<SpriteRenderer>().enabled = false;
		Instantiate(Resources.Load("Prefabs/Particles/BloodSplat"), transform.position, Quaternion.identity);			
	}

	public int GetHealth() {
		return health < 0 ? 0 : health;
	}

	public void SpeedBoost() {
		if (!spedUp) {
			spedUp = true;
			maxSpeed += speedBoostAmount;
		}
		speedBoostTimer = Time.time;		
		m_audioManager.PlayOneShot(SpeedPickUp, .5f);
	}

	public void DamageBoost() {
		if (!dmgUp) {
			dmgUp = true;
			damage *= dmgBoostAmount;
		}
		dmgBoostTimer = Time.time;
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
		m_audioManager.PlayOneShot(GunCock, .5f);		
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
		m_audioManager.PlayOneShot(SwordDraw, .5f);		
		
	}
}
