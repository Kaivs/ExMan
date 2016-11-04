using UnityEngine;
using System.Collections;

public class Speed : MonoBehaviour {
	private Player pScript;
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player"){
			pScript = other.gameObject.GetComponent<Player>();
			pScript.pickupSpeed = 1;		
			pScript.SpeedBoost();
			Destroy(this.gameObject);
		}
	}
}
