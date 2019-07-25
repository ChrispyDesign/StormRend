using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject m_gameUI;
    [SerializeField] private GameObject m_settingsMenu;
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_gameOver;
    [SerializeField] private GameObject m_gameWin;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !m_gameWin.activeSelf)
        {
            m_gameUI.SetActive(false);
            m_pauseMenu.SetActive(false);
            m_settingsMenu.SetActive(false);
            m_gameOver.SetActive(true);
            m_gameWin.SetActive(false);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
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
}
