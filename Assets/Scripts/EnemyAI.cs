using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {


	// Spawn-related Variable and Accessors/Mutators
	bool m_isActive;


	public bool IsActive { get { return m_isActive; } }
	public void SetActive() { m_isActive = true; }
	public void SetInactive() { m_isActive = false; }


	enum State { Seek, Dead }
	enum MovementSpeed { None, Default, Slow }

	[SerializeField] Transform m_targetTransform;
	[SerializeField] Vector3 m_targetPosition;

	[SerializeField] Transform m_transform;
	[SerializeField] float m_speed;
	[SerializeField] float m_turnSpeed;
	
	[SerializeField] int m_damage;
	[SerializeField] int m_health;

	[SerializeField] float m_attackSpeed;
	const float ATTACK_COOLDOWN_MAX = 5f;
	float m_attackCooldown = 0;

	// Unity Components
	Rigidbody2D m_rb2d;
	Animator m_anim;

	// Non-Unity Components
	const float DEATH_TIME_MAX = 2f;
	float m_deathCountdown;
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
			UpdateAnimation();
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
		if (m_attackCooldown <= 0) {
			m_attackCooldown = ATTACK_COOLDOWN_MAX;
			m_anim.SetTrigger("attack");
			GameManager.Instance.Player.LoseHealth(m_damage);
		}
		else {
			m_attackCooldown -= m_attackSpeed * Time.deltaTime;
		}
	}

	void UpdateAnimation() {

		m_anim.SetBool("isDead", m_isDead);
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
				Stop();
			}
			else {
				MoveForward();
			}	
		}
		else {
			
			Stop();
			Decay();
		}
	}

	void Decay() {
		m_deathCountdown -= Time.deltaTime;
		
		if (m_deathCountdown <= 0) {
			Despawn();
		}
	}

	void Stop() {

		m_rb2d.velocity = Vector2.zero;
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
			m_transform.rotation = Quaternion.RotateTowards(m_transform.rotation, rotationNeeded, m_turnSpeed);
		}

		return isRotating;
	}

	// SPAWN Related
	public void Spawn(Vector3 position) {

		m_isActive = true;
		transform.position = position;
		transform.rotation = Quaternion.LookRotation(Vector3.forward, m_targetPosition - m_transform.position);
	}

	public void Despawn() {
<<<<<<< HEAD
		GameObject.Find("GameManager").GetComponent<GameManager>().IncrementKills();
=======

>>>>>>> Enemy
		m_isActive = false;
		transform.position = transform.parent.transform.position;	

		// RE-INITIALIZE AFTER	
		Initialize();
	}

	void CheckIfDead() {

		if (m_health <= 0) {
			m_isDead = true;
			m_behaviour = State.Dead;
			m_deathCountdown = DEATH_TIME_MAX;
		}
	}

	public void LoseHealth(int newValue) {
		m_health -= newValue;
		CheckIfDead();
	}
}
