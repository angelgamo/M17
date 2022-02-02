using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartNewGame()
	{
		SaveLoad.instance.ResetGame();
		SceneManager.LoadScene("OpenWorld");
	}

	public void ContinueGame()
	{
		SceneManager.LoadScene("OpenWorld");
	}

	public void LoadOpenWorld()
	{
		SaveLoad.instance.SaveGame();
		SceneManager.LoadScene("OpenWorld");
	}

	public void LoadDungeon()
	{
		SaveLoad.instance.SaveGame();
		SceneManager.LoadScene("Dungeon");
	}
}
