using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject player;
	public float moveSpeed = 0.5f;
	private Vector3 moveDirection;
	private bool isSpawned;

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

	public void Spawn() {
		isSpawned = true;
		transform.position = player.transform.position;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		moveDirection.x = (mousePos.x - transform.position.x);
		moveDirection.y = (mousePos.y - transform.position.y);
		moveDirection = moveDirection.normalized;
	}

	void Despawn() {
		isSpawned = false;
		transform.position = new Vector3( -200, -200, 0);
	}

	public void SetActive(bool active) {
		isSpawned = active;
	}

	public bool GetActive() {
		return isSpawned;
	}
}
