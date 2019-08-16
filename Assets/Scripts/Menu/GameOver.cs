using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject m_gameUI;
    [SerializeField] GameObject m_settingsMenu;
    [SerializeField] GameObject m_pauseMenu;
    [SerializeField] GameObject m_gameOver;
    [SerializeField] GameObject m_gameWin;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Settings()
    {
        this.GetComponent<SettingsMenu>().m_parentMenu = m_gameOver;
        m_gameUI.SetActive(false);
        m_pauseMenu.SetActive(false);
        m_settingsMenu.SetActive(true);
        m_gameOver.SetActive(false);
        m_gameWin.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

	public void ShowScreen()
	{
		m_gameUI.SetActive(false);
		m_pauseMenu.SetActive(false);
		m_settingsMenu.SetActive(false);
		m_gameOver.SetActive(true);
		m_gameWin.SetActive(false);
	}
}