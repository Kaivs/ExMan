
// Script Name:		GameManager C# Script
// Created by: 		Nestor Sirilan Jr.
// Date Created: 	11/02/2016

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	static GameManager m_instance = null;
	public static GameManager Instance { get { return m_instance; } }



	
	int m_playerBulletPool = 20;
	public int PlayerBulletPool { get { return m_playerBulletPool; } }
	int m_enemyRatPool = 10;
	public int EnemyRatPool { get { return m_enemyRatPool; } }
	int m_enemyCockroachPool = 10;
	public int EnemyCockroackPool { get { return m_enemyCockroachPool; } }

	public GameObject[] prefabs;
	public Transform m_gameObjectsPool;

	public GameObject[] CreateObjectPool(string name, int count) {

		GameObject[] pool = new GameObject[count];

		bool found = false;

		for (int i = 0; i < prefabs.Length && !found; i++) {
			if (prefabs[i].name.Equals(name)) {
				for (int j = 0; j < pool.Length; j++) {
					pool[j] = Instantiate(prefabs[i], new Vector3(-200, -200, 0), Quaternion.identity, m_gameObjectsPool) as GameObject;
					pool[j].GetComponent<Bullet>().SetActive(false);
				}
				found = true;
			}
		}
		
		return pool;
	}





//==========================================================================
//		Game-Related Variables and Accessors/Mutators - BEGIN
//==========================================================================


	private Player m_player;
	public Player GetPlayer { get { return m_player; } }
	
	// *** Player Checker *** //
	public bool IsPlayerDead { get { return false /*To be changed soon!*/ ; } }



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
		//lol
	}


	void Start() {

		AcquireReferences();
		Initialize();
	}


//===========================================================================
//		MonoBehaviour-Related Functions - END
//===========================================================================



//===========================================================================
//		GameManager Functions - BEGIN
//===========================================================================

	void AcquireReferences() {

		m_gameObjectsPool = GameObject.FindGameObjectWithTag("Pool").GetComponent<Transform>();
		m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	void Initialize() {

		m_score = 0;
		m_wave = 0;
		m_isGameOver = false;
		// More variables soon
	}

	void UpdateUI() {
		// TODO: Functions here
	}

	void NextWave() {
		// TODO: Handles wave transitioning
	}

	void CheckGameOverCondition() {
		// TODO: Handles the gameOver condition
	}

//===========================================================================
//		GameManager Functions - END
//===========================================================================
}
