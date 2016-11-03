using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour {

	//Drag a reference to the audio source which will play the sound effects.
	public AudioSource m_sfxSource;

	//Drag a reference to the audio source which will play the music.
	public AudioSource m_musicSource;

	//Allows other scripts to call functions from AudioManager
	public static AudioManager instance = null;

	void Awake(){
		//Check if there is already an instance of AudioManager
		if(instance == null){
			//If there isn't, set it to this.
			instance = this;
		} else if (instance != this){
			//Destroy this, this enforces our singleton pattern so there can only be one instance
			Destroy(gameObject);
		}

		//Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

	//This is used to play single sound clips
	public void PlaySingle(AudioClip clip){
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		m_sfxSource.clip = clip;

		//Play the clip
		m_sfxSource.Play();
	}
}
