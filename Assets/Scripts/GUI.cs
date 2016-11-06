using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI : MonoBehaviour {
	private Player m_player;
	public int m_currentWave;
	public Sprite[] HeartSprites;
	public Sprite[] Weapon;
	public Sprite[] Speed;
	public Image SpeedUI;
	public Image HeartUI;
	public Image WeaponUI;

	// public int PlayerHealth{
	// 	get{ return m_player.Health;}
	// 	set{ m_player.Health = value;}
	// }

	// public string PlayerWeapon{
	// 	get { return m_player.Weapon;}
	// 	set { m_player.Weapon = value;}
	// }

	// public int CurrentWave{
	// 	get{return m_currentWave;}
	// 	set{m_currentWave = value;}
	// }

	// void DecreaseHealth(){
	// 	if(m_player.takingDamage){

	// 	}
	// }


	// Use this for initialization
	void Start () {
		m_player = GameObject.Find("Player").GetComponent<Player>();
	
	}
	
	// Update is called once per frame
	void Update () {
		HeartUI.sprite = HeartSprites[m_player.GetHealth()];
		WeaponUI.sprite = Weapon[m_player.Weapon];
		SpeedUI.sprite = Speed[m_player.pickupSpeed];
	}
}
