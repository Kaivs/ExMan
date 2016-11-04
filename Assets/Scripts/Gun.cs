using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	private Player pScript;
	void Start(){
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			pScript = other.gameObject.GetComponent<Player>();
			pScript.PickupGun();
			Destroy(this.gameObject);
		}
	}
}
