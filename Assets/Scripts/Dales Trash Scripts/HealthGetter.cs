using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StormRend;

public class HealthGetter : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Unit player;
    [SerializeField] float health;


    private void Awake()
    {
       health = player.HP;   
    }
    private void Update()
    {
        health = (player.HP / (float)player.maxHP);

        healthBar.fillAmount = health;
    }
}
