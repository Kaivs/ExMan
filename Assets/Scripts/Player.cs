using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Rigidbody2D rb2d;
	public GameObject bullet;
	public float maxSpeed = 10;
	public bool hasGun = true;
	public bool hasSword = false;
	private Quaternion rotation;
	private GameObject[] BulletPool;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		BulletPool = new GameObject[20];
		CreateBulletPool();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		
		// Movement Keys
		float hInput = Input.GetAxis ("Horizontal");
		float vInput = Input.GetAxis ("Vertical");
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		bool attack = Input.GetMouseButtonDown(0);

		if(attack) {
			Attack();
		}

		//Rotation to mousePos
		rb2d.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x))*Mathf.Rad2Deg - 90);
		rb2d.velocity = new Vector2 (hInput * maxSpeed, vInput * maxSpeed);
	}

	void Attack() {
		if (hasGun) {
			Shoot();
			GetComponent<Animator>().SetTrigger("shooting");
		} else if (hasSword) {
			GetComponent<Animator>().SetTrigger("slashing");
			//Attack animation for sword
		} else {
			//Attack animation for fists
		}
	}

	void CreateBulletPool() {
		for (int i = 0; i < BulletPool.Length; i++) {
			BulletPool[i] = Instantiate(bullet, new Vector3(-200, -200, 0), Quaternion.identity) as GameObject;
			BulletPool[i].GetComponent<Bullet>().SetActive(false);
		}
	}

	void Shoot() {
		for (int i = 0; i < BulletPool.Length; i++) {
			if (!BulletPool[i].GetComponent<Bullet>().GetActive()) {
				BulletPool[i].GetComponent<Bullet>().Spawn();
				return;
			}
		}
	}

	public Quaternion GetRotation() {
		return rotation;
	}
}
