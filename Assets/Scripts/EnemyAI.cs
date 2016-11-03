using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {


	// Spawn-related Variable and Accessors/Mutators
	bool m_isActive;
	public bool IsActive { get { return m_isActive; } }
	public void SetActive() { m_isActive = true; }
	public void SetInactive() { m_isActive = false; }


	enum Behaviour { Seek, Attack }

	[SerializeField] Transform m_targetTransform;
	[SerializeField] Vector3 m_targetPosition;

	[SerializeField] float m_speed;
	[SerializeField] float m_turnSpeed;
	
	[SerializeField] int m_damage;
	[SerializeField] int m_health;

	// Unity Components
	Rigidbody2D m_rb2d;
	Animator m_anim;
	bool m_isDead;

	void Start() {
		
		AcquireReference();
		Initialize();
	}

	void AcquireReference() {

		m_rb2d = GetComponent<Rigidbody2D>();
		m_anim = transform.GetChild(0).GetComponent<Animator>();
	}

	void Initialize() {

		m_isDead = false;
	}


}
