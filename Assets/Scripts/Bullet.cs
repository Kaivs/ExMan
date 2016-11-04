using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public enum Ownership { Player, Enemy }
	public GameObject player;
	public float moveSpeed = 0.5f;
	private Vector3 moveDirection;
	private bool isSpawned;
	private Ownership m_type;
	private int m_damage;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (isSpawned) {
			transform.position += (moveDirection * Time.deltaTime) * moveSpeed;
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.gameObject.tag == "MainCamera") {
			Despawn();
		}
	}

	public void Spawn(Vector3 position, Vector3 target, Ownership type, int damage, bool isMouseLocation = false) {
		m_damage = damage;
		m_type = type;
		SetActive();
		transform.position = position;

		if (isMouseLocation) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(target);
			moveDirection.x = (mousePos.x - transform.position.x);
			moveDirection.y = (mousePos.y - transform.position.y);
			transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mousePos.y - transform.position.y), (mousePos.x - transform.position.x))*Mathf.Rad2Deg - 90); 
		}
		else {
			moveDirection.x = (target.x - transform.position.x);
			moveDirection.y = (target.y - transform.position.y);
		}

		moveDirection = moveDirection.normalized;
	}

	void Despawn() {
		SetInactive();
		transform.position = transform.parent.transform.position;
	}

	public void SetActive() {
		isSpawned = true;
	}

	public void SetInactive() {
		isSpawned = false;
	}

	public bool GetActive() {
		return isSpawned;
	}




	void OnTriggerEnter2D(Collider2D other)
	{
		CheckTriggerCollision(other);

	}
	void OnTriggerStay2D(Collider2D other)
	{
		CheckTriggerCollision(other);
	}

	void CheckTriggerCollision(Collider2D other) {

		bool deSpawn = false;

		if (other.tag == "Player" && m_type.Equals(Ownership.Enemy) && isSpawned) {
			other.gameObject.GetComponent<Player>().LoseHealth(m_damage);
		}
		else if (other.tag == "Enemy" && m_type.Equals(Ownership.Player) && isSpawned) {
			other.gameObject.GetComponent<EnemyAI>().LoseHealth(m_damage);
		}

		if (deSpawn) {
			Despawn();
		}
	}
}
