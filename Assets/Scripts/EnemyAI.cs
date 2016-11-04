using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {


	// Spawn-related Variable and Accessors/Mutators
	bool m_isActive;


	public bool IsActive { get { return m_isActive; } }
	public void SetActive() { m_isActive = true; }
	public void SetInactive() { m_isActive = false; }


	enum State { Seek, Attack, Dead }
	enum MovementSpeed { None, Default, Slow }

	[SerializeField] Transform m_targetTransform;
	[SerializeField] Vector3 m_targetPosition;

	[SerializeField] Transform m_transform;
	[SerializeField] float m_speed;
	[SerializeField] float m_turnSpeed;
	
	[SerializeField] int m_damage;
	[SerializeField] int m_health;

	// Unity Components
	Rigidbody2D m_rb2d;
	Animator m_anim;

	// Non-Unity Components
	bool m_isDead;
	State m_behaviour;
	MovementSpeed m_speedType;


//=====================================================
//		MonoBehaviour-Related Functions :: BEGIN
//=====================================================

	void Start() {
		
		AcquireReference();
		Initialize();
	}
	
	void Update() {

		if (m_isActive) {

			UpdateTargetInformation();
			InvokeBehaviour();
		}
		else {
			if (m_targetTransform == null) {
				m_targetTransform = GameManager.Instance.Player.GetComponent<Transform>();
			}
		}
	}
	
//=====================================================
//		MonoBehaviour-Related Functions :: END
//=====================================================

	void AcquireReference() {

		m_rb2d = GetComponent<Rigidbody2D>();
		m_anim = transform.GetChild(0).GetComponent<Animator>();
		
		m_transform = GetComponent<Transform>();
	}

	void Initialize() {

		m_isDead = false;
		m_isActive = false;
		
		m_behaviour = State.Seek;
		m_speedType = MovementSpeed.Default;
	}

	void UpdateTargetInformation() {

		m_targetPosition = m_targetTransform.position;
	}

	void Attack() {
		m_anim.SetTrigger("attack");
	}

	void InvokeBehaviour() {

		if (m_behaviour.Equals(State.Seek)) {

			if (RotateTowardsTarget()) {
				m_speedType = MovementSpeed.Slow;
			}

			if (Vector2.Distance(m_targetPosition, m_transform.position) < 0.5f) {
				m_speedType = MovementSpeed.None;
				Attack();
			}
			else {
				m_speedType = MovementSpeed.Default;
			}

			if (m_speedType.Equals(MovementSpeed.None)) {
				m_rb2d.velocity = Vector2.zero;
			}
			else {
				MoveForward();
			}	
		}
	}

	void MoveForward() { MoveForward(m_speed); }

	void MoveForward(float speed) { 

		if (m_speedType.Equals(MovementSpeed.Slow)) {
			speed /= 2f;
		}
		else if (m_speedType.Equals(MovementSpeed.None)) {
			m_speed = 0;
		}
		
		m_rb2d.AddRelativeForce(Vector2.up * speed);
	}

	bool RotateTowardsTarget() { return RotateTowards(m_targetPosition); }

	bool RotateTowards(Vector2 location) {

		bool isRotating = false;

		Quaternion rotationNeeded = Quaternion.LookRotation(Vector3.forward, m_targetPosition - m_transform.position);

		if (m_transform.rotation != rotationNeeded) {
			isRotating = true;
			m_transform.rotation = Quaternion.RotateTowards(m_transform.rotation, rotationNeeded, m_turnSpeed * Time.deltaTime);
		}


		return isRotating;
	}

	// SPAWN Related
	public void Spawn(Vector3 position) {
		m_isActive = true;
		transform.position = position;
		transform.rotation = Quaternion.LookRotation(Vector3.forward, m_targetPosition - m_transform.position);
	}

	void Despawn() {
		m_isActive = false;
		transform.position = transform.parent.transform.position;
	}
}
