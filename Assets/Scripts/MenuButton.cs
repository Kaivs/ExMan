using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {


	[SerializeField] Text m_score;
	[SerializeField] Text m_hScore;


	void Start() {

		if (SceneManager.GetActiveScene().name.Equals("GameOver")) {
			int score = GameManager.Instance.CurrentWave;

			m_score.text = m_score.text + " " + score;
			
			int highscore = PlayerPrefs.GetInt("highscore");

			if (score > highscore) {
				PlayerPrefs.SetInt("highscore", score);
			}

			highscore = PlayerPrefs.GetInt("highscore");

			m_hScore.text = m_hScore.text + " " + highscore;
		}
	}


	public void GoToMenuFromSplash() {
		Invoke("GoToMenu", 1);
	}
	public void GoToMenu() {
		ResetGameManager();
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToPlay() {
		ResetGameManager();
		SceneManager.LoadScene("Main");
	}	

	public void GoToControls() {
		SceneManager.LoadScene("Controls");
	}

	public void ExitGame() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
		Application.Quit();
	}

	void ResetGameManager() {
		if (GameManager.Instance != null) {
			DestroyObject(GameManager.Instance.gameObject);
		}
	}
}

