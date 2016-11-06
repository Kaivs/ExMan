// Script Name:		GameManager C# Script
// Created by: 		Nestor Sirilan Jr.
// Date Created: 	11/02/2016

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	static GameManager m_instance = null;
	public static GameManager Instance { get { return m_instance; } }



	enum EnemyType { Rat, Cockroach }	
	EnemyType GetRandomEnemy() {
		EnemyType type;
		int enemy = Random.Range(0, 2);

		switch (enemy) {
			case 0:
			type = EnemyType.Rat;
			break;
			case 1:
			type = EnemyType.Cockroach;
			break;
			default:
			type = EnemyType.Rat;
			break;
		}

		return type;
	}
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


    private Collider2D m_background;
    private float m_pickupTimer;
	public int m_activeEnemy;
	public bool m_spawnEnemy;




//==========================================================================
//		Game-Related Variables and Accessors/Mutators - BEGIN
//==========================================================================


	private Player m_player;
	public Player Player { get { return m_player; } }	
	public bool IsPlayerDead { get { return false /*To be changed soon!*/ ; } }
	private int m_killCount;
	public int KillCount { get { return m_killCount; } }
	public void IncrementKillCount() { m_killCount += 1; }



	bool m_isGameOver;
	public bool IsGameOver { get { return m_isGameOver; } }
	int m_score;
	public int CurrentScore { get { return m_score; } }
	public void IncrementScore(int newValue) { m_score += newValue; }
	public void DecrementScore(int newValue) { m_score -= newValue; }
	int m_wave;
	public int CurrentWave { get { return m_wave; } }
	public void IncrementWave() { IncrementWave(1); }
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

		// IF :: PLAYING = TRUE
		if (m_isPlaying) {

			SpawnPickups();

			if (m_spawnEnemy) {		
				for (int i = 0; i < m_wave; i++) {		
					Spawn(GetRandomEnemy());
				}
				m_spawnEnemy = false;
			}
			if (m_activeEnemy <= 0) {
				m_spawnEnemy = true;
				m_wave += 1;
			}
		}
		// IF :: PLAYING = FALSE
		else {

			m_countdown -= Time.deltaTime;
			if (m_countdown <= 0) {
				m_isPlaying = true;
				Spawn(GetRandomEnemy());
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
			SpawnFromPool(m_enemyRatPool);
		}
		else if (type.Equals(EnemyType.Cockroach)) {
			SpawnFromPool(m_enemyCockroachPool);			
		}
	}

	void SpawnFromPool(GameObject[] pool) {
		for (int i = 0; i < pool.Length; i++) {
			if (!pool[i].GetComponent<EnemyAI>().IsActive) {
				pool[i].GetComponent<EnemyAI>().Spawn(GetRandomWorldLocation());
				return;
			}
		}
	}

	void CreateEnemyPool() {
		m_enemyRatPool = CreateObjectPool("enemy_rat", EnemyRatPoolCount);
		m_enemyCockroachPool = CreateObjectPool("enemy_cockroach", EnemyCockroackPoolCount);
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
		m_pickupTimer = 0;
		m_spawnEnemy = false;
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
    	Vector2 randomPos = new Vector2(Random.Range(m_background.bounds.center.x - m_background.bounds.extents.x, m_background.bounds.center.x + m_background.bounds.extents.x),
										Random.Range(m_background.bounds.center.y - m_background.bounds.extents.y, m_background.bounds.center.y + m_background.bounds.extents.y));
        if (Time.time - m_pickupTimer > 3) {
            switch(Random.Range(1,5)) {
                case 1:
                    //Gun pickup spawn              
                    Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Gun"), randomPos, Quaternion.identity);
                    break;
                case 2:
                    //Sword pickup spawn
                    Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Sword"), randomPos, Quaternion.identity);
                    break;
                case 3:
                    //Health pickup spawn
                    Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Health"), randomPos, Quaternion.identity);
                    break;
                case 4:
                    //Speed pickup spawn
                    Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Speed"), randomPos, Quaternion.identity);
                    break;
                case 5:
                    //Damage pickup spawn
                    Instantiate(Resources.Load("Prefabs/Pickups/Pickup - Damage"), randomPos, Quaternion.identity);
                    break;
            }
            m_pickupTimer = Time.time;
        }
    }
	Vector2 GetRandomWorldLocation() {
		return new Vector2(Random.Range(m_background.bounds.center.x - m_background.bounds.extents.x, m_background.bounds.size.x),
                                        Random.Range(m_background.bounds.center.y - m_background.bounds.extents.y, m_background.bounds.size.y));
	}

//===========================================================================
//		GameManager Functions - END
//===========================================================================
}