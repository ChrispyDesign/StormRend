using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_gameUI;
    [SerializeField] private GameObject m_settingsMenu;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_gameOver;
    [SerializeField] private GameObject m_gameWin;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GamePause();
    }

    public void GamePause()
    {
        if (!m_pauseMenu.activeSelf && 
            !m_gameOver.activeSelf && 
            !m_gameWin.activeSelf)
        {
            Time.timeScale = 0.0f;
            m_gameUI.SetActive(false);
            m_settingsMenu.SetActive(false);
            m_pauseMenu.SetActive(true);
            m_gameOver.SetActive(false);
            m_gameWin.SetActive(false);
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
