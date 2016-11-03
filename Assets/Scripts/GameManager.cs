using UnityEngine;
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

