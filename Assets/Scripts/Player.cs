using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Rigidbody2D rb2d;
	public GameObject bullet;
	public float maxSpeed = 10;
	private bool hasGun = true;
	private bool hasSword = false;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
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
		} else if (hasSword) {
			//Attack animation for sword
		} else {
			//Attack animation for fists
		}
	}

	void Shoot() {
		Instantiate(bullet, this.transform.position, Quaternion.identity);
	}
}
