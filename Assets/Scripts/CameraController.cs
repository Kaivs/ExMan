using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public GameObject backgroundGO;
	private Collider2D background;
	private Vector3 playerPos;
	private float deadZoneX = 5;
	private float deadZoneY = 4;
	private Vector3 velocity = Vector3.zero;
	private float smoothTime = 1f;


	// Camera bounds restriction
	private float vCamExtent;
	private float hCamExtent;
	private float minX;
	private float maxX;
	private float minY;
	private float maxY;
	

	void Start () {
		playerPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		background = backgroundGO.GetComponent<BoxCollider2D>();
		vCamExtent = Camera.main.orthographicSize;
		hCamExtent = vCamExtent * Screen.width / Screen.height;

		minX = background.bounds.center.x - background.bounds.extents.x + hCamExtent;
		maxX = background.bounds.center.x + background.bounds.extents.x - hCamExtent;
		minY = background.bounds.center.y - background.bounds.extents.y + vCamExtent;
		maxY = background.bounds.center.y + background.bounds.extents.y - vCamExtent;
		// Get the background bound points
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Lerp to player
		if ((Mathf.Abs(player.transform.position.x - transform.position.x) > deadZoneX ) || (Mathf.Abs(player.transform.position.y - transform.position.y) > deadZoneY )) {
			// Keep camera within the bounds of the background
			playerPos = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, smoothTime);
			playerPos.z = transform.position.z;
			playerPos.x = Mathf.Clamp(playerPos.x, minX, maxX);
			playerPos.y = Mathf.Clamp(playerPos.y, minY, maxY);	
			transform.position = playerPos;
		}
	}
}
