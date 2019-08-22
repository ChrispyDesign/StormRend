using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StormRend;

public class HealthGetter : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Unit player;
    public int health;


    private void Awake()
    {
       health = player.HP;   
    }
    private void Update()
    {
        health = player.HP;

        text.text = health.ToString();
    }
}
