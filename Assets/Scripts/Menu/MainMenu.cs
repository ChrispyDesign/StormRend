using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private GameObject m_settingsMenu;
    
    public void Play()
    {
        Debug.Log("Scene Deleted");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings()
    {
        m_mainMenu.SetActive(false);
        m_settingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Application Deleted");
        Application.Quit();
    }
}