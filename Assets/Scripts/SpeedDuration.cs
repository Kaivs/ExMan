using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedDuration : MonoBehaviour {
	private Player m_player;

	private Slider m_slider;

	// Use this for initialization
	void Start () {
		m_player = GameObject.Find("Player").GetComponent<Player>();
		m_slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_player.spedUp){
			m_slider.maxValue = m_player.speedBoostTimer;
		}
	}
}
