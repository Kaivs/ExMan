
// Script Name:		GameManager C# Script
// Created by: 		Nestor Sirilan Jr.
// Date Created: 	11/02/2016

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	static GameManager m_instance = null;
	public static GameManager Instance { get { return m_instance; } }


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
	
	int m_playerBulletPool = 20;
	public int PlayerBulletPool { get { return m_playerBulletPool; } }
	int m_enemyRatPool = 10;
	public int EnemyRatPool { get { return m_enemyRatPool; } }
	int m_enemyCockroachPool = 10;
	public int EnemyCockroackPool { get { return m_enemyCockroachPool; } }

	public GameObject[] prefabs;

	public GameObject[] CreateObjectPool(string name, int count) {
		GameObject[] pool = new GameObject[count];

		bool found = false;

		for (int i = 0; i < prefabs.Length && !found; i++) {
			if (prefabs[i].name.Equals(name)) {
				for (int j = 0; j < pool.Length; j++) {
					pool[j] = Instantiate(prefabs[i], new Vector3(-200, -200, 0), Quaternion.identity) as GameObject;
					pool[j].GetComponent<Bullet>().SetActive(false);
				}
				found = true;
			}
		}
		
		return pool;
	}






	int m_wave;
	public int CurrentWave { get { return m_wave; } }
}
