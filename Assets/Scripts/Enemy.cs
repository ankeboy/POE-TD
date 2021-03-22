using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Enemy : MonoBehaviour
{
    private float Difficulty = 1f;  //for now there is no other difficulty planned.
    [HideInInspector]
    public float speed;
    [Header("Enemy stats")]
    public float startSpeed = 10f;
    public float baseHealth = 100;
    private float health;
    public int worth = 50;

    [Header("Unity Stuff")]
    public GameObject deathEffect;
    public Image healthBar;
    private bool isDead = false;

    void Start()
    {
        speed = startSpeed;
        health = baseHealth * (float)Math.Pow(WaveSpawner.roundMultiplier, WaveSpawner.waveIndex) * Difficulty; //math.pow returns a double. Need to cast it to float.
    }

    public void TakeDamage(float amount)    //void -> we dont want it to return anything
    {
        health -= amount;

        healthBar.fillAmount = health / baseHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die()
    {
        isDead = true;
        PlayerStats.Money += worth;

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);

    }

}
