using UnityEngine;
using System.Collections;

public class Speed : MonoBehaviour {
	private Player pScript;
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			pScript = other.gameObject.GetComponent<Player>();			
			pScript.SpeedBoost();
			Destroy(this.gameObject);
		}
	}
}
