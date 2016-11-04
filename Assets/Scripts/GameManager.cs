<<<<<<< HEAD
﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Reflection;

public class GameManager : MonoBehaviour {	
	public static GameManager instance = null;

	// Use this for initialization
	void Awake(){
	// 	//Check if there is already an instance of AudioManager
	// 	if(instance == null){
	// 		//If there isn't, set it to this.
	// 		instance = this;
	// 	} else if (instance != this){
	// 		//Destroy this, this enforces our singleton pattern so there can only be one instance
	// 		Destroy(gameObject);
	// 	}

	// 	//Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
	// 	DontDestroyOnLoad(gameObject);
	}

	public void LoadScene(string scene){
		SceneManager.LoadScene(scene);
	}

}

=======
﻿
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

		if (m_isPlaying) {


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
	}

	void Initialize() {

		m_score = 0;
		m_wave = 0;
		m_isGameOver = false;
		m_isPlaying = false;
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
>>>>>>> EnemyLol
