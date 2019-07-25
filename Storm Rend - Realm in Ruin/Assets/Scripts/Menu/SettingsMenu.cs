using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject m_parentMenu;
    [SerializeField] private GameObject m_settingsMenu;

    public void BackToParent()
    {
        m_settingsMenu.SetActive(false);
        m_parentMenu.SetActive(true);
    }
}
