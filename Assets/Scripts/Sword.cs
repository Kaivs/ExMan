using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	private Player pScript;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			pScript = other.gameObject.GetComponent<Player>();			
			pScript.PickupSword();
			Destroy(this.gameObject);
		}
	}
}
