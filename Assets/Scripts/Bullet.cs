using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private Vector3 moveDirection;
	public float moveSpeed = 20f;
	// Use this for initialization
	void Start () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		moveDirection = new Vector3(mousePos.x, mousePos.y, 0);
		moveDirection = moveDirection.normalized;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += (moveDirection * Time.deltaTime) * moveSpeed;
	}
}
