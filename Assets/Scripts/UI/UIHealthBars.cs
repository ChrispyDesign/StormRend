using System.Collections;
using System.Collections.Generic;
using StormRend;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBars : MonoBehaviour
{
    [SerializeField] private GameObject m_healthBarPrefab;

    private Dictionary<Unit, Slider> m_unitHealthBars;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            InitialiseHealthBars(FindObjectsOfType<Unit>());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="units"></param>
    private void InitialiseHealthBars(Unit[] units)
    {
        m_unitHealthBars = new Dictionary<Unit, Slider>();

        for (int i = 0; i < units.Length; i++)
        {
            Unit unit = units[i];
            int currentHP = unit.GetHP();
            int maxHP = unit.GetMaxHP();

            GameObject newHealthBar = Instantiate(m_healthBarPrefab);
            Slider healthBar = newHealthBar.GetComponent<Slider>();
            healthBar.value = currentHP / maxHP;

            m_unitHealthBars.Add(units[i], healthBar);
        }
    }

    private void UpdateHealthBar(Unit unit)
    {

    }
}
