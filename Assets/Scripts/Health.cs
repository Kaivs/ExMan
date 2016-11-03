using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	private Player pScript;
	private int healAmount = 1;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			pScript = other.gameObject.GetComponent<Player>();			
			pScript.AddHealth(healAmount);
			Destroy(this.gameObject);
		}
	}
}
