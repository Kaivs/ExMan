using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

	public void GoToMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void GoToPlay() {
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
}