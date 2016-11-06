// Script Name:		GameManager C# Script
// Created by: 		Nestor Sirilan Jr.
// Date Created: 	11/02/2016

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	static GameManager m_instance = null;
	public static GameManager Instance { get { return m_instance; } }



	enum EnemyType { Rat }	
	int m_playerBulletPoolCount = 20;
	public int PlayerBulletPoolCount { get { return m_playerBulletPoolCount; } }
	GameObject[] m_enemyRatPool;
	int m_enemyRatPoolCount = 10;
	public int EnemyRatPoolCount { get { return m_enemyRatPoolCount; } }
	GameObject[] m_enemyCockroachPool;
	int m_enemyCockroachPoolCount = 10;
	public int EnemyCockroackPoolCount { get { return m_enemyCockroachPoolCount; } }
	public GameObject[] prefabs;
	public Transform m_gameObjectsPool;

	// References for spawning prefabs
	private GameObject m_gunPickup;
	private GameObject m_swordPickup;
	private GameObject m_healthPickup;
	private GameObject m_speedPickup;
	private GameObject m_dmgPickup;
	private Collider2D m_background;
	private float pickupTimer = 0;

	public GameObject[] CreateObjectPool(string name, int count) {

		GameObject[] pool = new GameObject[count];

		bool found = false;

		if (name.StartsWith("bullet_")) {
			for (int i = 0; i < prefabs.Length && !found; i++) {
				if (prefabs[i].name.Equals(name)) {
					for (int j = 0; j < pool.Length; j++) {
						pool[j] = Instantiate(prefabs[i], m_gameObjectsPool.position, Quaternion.identity, m_gameObjectsPool) as GameObject;
						pool[j].GetComponent<Bullet>().SetInactive();
					}
					found = true;
				}
			}
		}
		else if (name.StartsWith("enemy_")) {
			for (int i = 0; i < prefabs.Length && !found; i++) {
				if (prefabs[i].name.Equals(name)) {
					for (int j = 0; j < pool.Length; j++) {
						pool[j] = Instantiate(prefabs[i], m_gameObjectsPool.position, Quaternion.identity, m_gameObjectsPool) as GameObject;
						pool[j].GetComponent<EnemyAI>().SetInactive();
					}
					found = true;
				}
			}
		}
		
		return pool;
	}





//==========================================================================
//		Game-Related Variables and Accessors/Mutators - BEGIN
//==========================================================================


	private Player m_player;
	public Player Player { get { return m_player; } }
	
	// *** Player Checker *** //
	public bool IsPlayerDead { get { return false /*To be changed soon!*/ ; } }



	bool m_isGameOver;
	public bool IsGameOver { get { return m_isGameOver; } }
	int m_score;
	public int CurrentScore { get { return m_score; } }
	int m_wave;
	public int CurrentWave { get { return m_wave; } }

	int m_killCount;
	public void IncrementWave() { 
		if (m_killCount * m_wave > 10 * m_wave) {
			IncrementWave(1); 
		}
	}

	public void IncrementKills() { m_killCount += 1; }
	public void IncrementWave(int newValue) { m_wave += newValue; }
	public void DecrementWave() { DecrementWave(1); }
	public void DecrementWave(int newValue) { m_wave -= newValue; }

	

	public float m_countdown;
	public bool m_isPlaying;

	

//==========================================================================
//		Game-Related Variables and Accessors/Mutators - END
//==========================================================================

//===========================================================================
//		MonoBehaviour-Related Functions - BEGIN
//===========================================================================

	void Awake() {

		if (Instance != null) {
			DestroyObject(this.gameObject);
		}
		else {
			m_instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Start() {

		AcquireReferences();
		Initialize();
		
		CreateEnemyPool();
	}

	void Update() {

		if (m_isPlaying) {
			SpawnPickups();
			IncrementWave();
		}
		else {
			m_countdown -= Time.deltaTime;
			if (m_countdown <= 0) {
				m_isPlaying = true;
				Spawn(EnemyType.Rat);
			}
		}
	}


//===========================================================================
//		MonoBehaviour-Related Functions - END
//===========================================================================



// (***************) LEVEL RELATED (***************)
	
	void ManageWave() {

	}



//===========================================================================
//		GameManager Functions - BEGIN
//===========================================================================

	void Spawn(EnemyType type) {
		
		if (type.Equals(EnemyType.Rat)) {
			for (int i = 0; i < m_enemyRatPool.Length; i++) {
				if (!m_enemyRatPool[i].GetComponent<EnemyAI>().IsActive) {
					m_enemyRatPool[i].GetComponent<EnemyAI>().Spawn(transform.position);
					return;
				}
			}
		}
	}

	void CreateEnemyPool() {
		m_enemyRatPool = CreateObjectPool("enemy_rat", EnemyRatPoolCount);
	}

	void AcquireReferences() {

		m_gameObjectsPool = GameObject.FindGameObjectWithTag("Pool").GetComponent<Transform>();
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();	
		m_background = GameObject.Find("Background").GetComponent<Collider2D>();	
	}

	void Initialize() {

		m_score = 0;
		m_wave = 1;
		m_isGameOver = false;
		m_isPlaying = false;
		// More variables soon
	}

	void UpdateUI() {
		// TODO: Functions here
	}

	void NextWave() {
		
		m_wave += 1;

	}

	void CheckGameOverCondition() {
		// TODO: Handles the gameOver condition
	}

	void SpawnPickups() {
		if (Time.time - pickupTimer > 15) {

			Vector2 randomPos = new Vector2(Random.Range(m_background.bounds.center.x - m_background.bounds.extents.x, m_background.bounds.size.x),
											Random.Range(m_background.bounds.center.y - m_background.bounds.extents.y, m_background.bounds.size.y));

			switch(Random.Range(1,5)) {
				case 1:
					//Gun pickup spawn				
					m_gunPickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Gun"), randomPos, Quaternion.identity) as GameObject;
					break;
				case 2:
					//Sword pickup spawn
					m_swordPickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Sword"), randomPos, Quaternion.identity) as GameObject;
					break;
				case 3:
					//Health pickup spawn
					m_healthPickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Health"), randomPos, Quaternion.identity) as GameObject;
					break;
				case 4:
					//Speed pickup spawn
					m_speedPickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Speed"), randomPos, Quaternion.identity) as GameObject;
					break;
				case 5:
					//Damage pickup spawn
					m_dmgPickup = Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Damage"), randomPos, Quaternion.identity) as GameObject;
					break;
			}
			pickupTimer = Time.time;

		}
	}

//===========================================================================
//		GameManager Functions - END
//===========================================================================
}