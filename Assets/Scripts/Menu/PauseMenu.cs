using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StormRend;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_gameUI;
    [SerializeField] private GameObject m_settingsMenu;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_gameOver;
    [SerializeField] private GameObject m_gameWin;
	[SerializeField] private Unit m_currentPlayer;
	public bool isPauseMenuShown = false;


    public void GamePause()
    {
        if (!m_gameOver.activeSelf && 
            !m_gameWin.activeSelf)
        {
            Time.timeScale = 0.0f;
            m_gameUI.SetActive(isPauseMenuShown);
            m_settingsMenu.SetActive(false);
            m_pauseMenu.SetActive(!isPauseMenuShown);
            m_gameOver.SetActive(false);
            m_gameWin.SetActive(false);
			isPauseMenuShown = !isPauseMenuShown;
        }
    }

    public void BackToGame()
    {
        Time.timeScale = 1.0f;
        m_gameUI.SetActive(true);
        m_settingsMenu.SetActive(false);
        m_pauseMenu.SetActive(false);
        m_gameOver.SetActive(false);
        m_gameWin.SetActive(false);
    }

    public void Settings()
    {
        this.GetComponent<SettingsMenu>().m_parentMenu = m_pauseMenu;
        m_gameUI.SetActive(false);
        m_pauseMenu.SetActive(false);
        m_settingsMenu.SetActive(true);
        m_gameOver.SetActive(false);
        m_gameWin.SetActive(false);
    }

}
