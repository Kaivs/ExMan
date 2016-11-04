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
		Spawn(player.transform.position, Input.mousePosition, true);
	}

	public void Spawn(Vector3 position, Vector3 target, bool isMouseLocation = false) {
		isSpawned = true;
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
		isSpawned = false;
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
}
