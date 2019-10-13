using System.Collections;
using System.Collections.Generic;
using StormRend;
using StormRend.Defunct;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBars : MonoBehaviour
{
    [SerializeField] private GameObject m_healthBarPrefab;

    private Dictionary<xUnit, Slider> m_unitHealthBars;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            InitialiseHealthBars(FindObjectsOfType<xUnit>());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="units"></param>
    private void InitialiseHealthBars(xUnit[] units)
    {
        m_unitHealthBars = new Dictionary<xUnit, Slider>();

        for (int i = 0; i < units.Length; i++)
        {
            xUnit unit = units[i];
            int currentHP = unit.HP;
            int maxHP = unit.maxHP;

            GameObject newHealthBar = Instantiate(m_healthBarPrefab);
            Slider healthBar = newHealthBar.GetComponent<Slider>();
            healthBar.value = currentHP / maxHP;

            m_unitHealthBars.Add(units[i], healthBar);
        }
    }

    private void UpdateHealthBar(xUnit unit)
    {

    }
}
