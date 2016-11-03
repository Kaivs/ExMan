using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public Sprite background;
	private Vector3 playerPos;
	private float deadZoneX = 5;
	private float deadZoneY = 4;
	private Vector3 velocity = Vector3.zero;
	private float smoothTime = 1f;
	// Use this for initialization
	void Start () {
		playerPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

		// Get the background bound points
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Lerp to player
		if ((Mathf.Abs(player.transform.position.x - transform.position.x) > deadZoneX ) || (Mathf.Abs(player.transform.position.y - transform.position.y) > deadZoneY )) {
			// Keep camera within the bounds of the background
			//if (transform.position.x < background.bounds.)
			playerPos = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, smoothTime);
			playerPos.z = transform.position.z;
			transform.position = playerPos;
		}
	}
}
