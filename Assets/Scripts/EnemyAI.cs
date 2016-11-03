using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	enum Behaviour { Seek, Attack }

	public Transform m_targetTransform;
	public Vector3 m_targetPosition;

	public float m_speed;
	public float m_turnSpeed;
	
	public int m_damage;
	public int m_health;

	// Unity Components
	Rigidbody2D m_rb2d;
	bool m_isDead;

	void Start() {
		
		// Unity Components
		m_rb2d = GetComponent<Rigidbody2D>();

		// TODO: Your own initializer
		m_isDead = false;
	}
}
