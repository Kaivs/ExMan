using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponDurability : MonoBehaviour {

	private Player m_player;

	private Slider m_slider; 	

	// Use this for initialization
	void Start () {
		m_player = GameObject.Find("Player").GetComponent<Player>();
		m_slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		m_slider.maxValue = 20;
		m_slider.minValue = 0;
		if(m_player.hasGun){
			m_slider.value = m_player.bulletCounter;
		} else {
			m_slider.value = m_player.swordCounter;
		}

	}
}
