﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class WaveSpawner : MonoBehaviour
{
    public Text roundsText;

    [HideInInspector]
    public static int EnemiesAlive = 0; //static: allows it to change 
    public Wave[] waves;
    [Header("Round settings")]
    public static float roundMultiplier = 1.15f; //health multiplier per round
    float timeBetweenWaves = 5f;
    private float countdown = 3f;
    //public int roundBonus = 1;
    [Header("References")]
    public Transform spawnPoint;
    //PauseMenu pausemenu;
    public Text waveCountdownText;
    public GameManager gameManager;
    public static int waveIndex = 0;
    private bool triggerBonus = false;

    void Start()
    {
        roundsText.text = "ROUND 1";        //initial round
        EnemiesAlive = 0;                   //Need to call this since Otherwise enemies alive are a positive integer when restarting the scene and game doesnt continue.
        StartCoroutine(RoundBonusSequence());
    }

    void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            gameManager.WinLevel();
            this.enabled = false;
            triggerBonus = false;
        }

        if (triggerBonus)
        {
            if (PlayerStats.Lives <= 0)
            {
                triggerBonus = false;
                return;
            }
            Debug.Log("trigger first bonus");
            StartCoroutine(RoundBonusSequence());

            PlayerStats.Money += (PlayerPrefs.GetInt("Money Round Bonus", 0) * 100);
            triggerBonus = false;
        }

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        roundsText.text = "ROUND " + (waveIndex + 1).ToString();    //After last enemy is defeated before countdown starts

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:0.00}", countdown); //curly brackets controls format
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];

        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        triggerBonus = true;
        waveIndex++;

    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }

    IEnumerator RoundBonusSequence()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.RoundBonusItem();

        if (GameObject.Find("BonusHouse(Clone)"))                      //if it finds a BonusHouse gameobject, add another option.
        {
            Debug.Log("trigger second bonus");
            yield return new WaitForSeconds(0.2f);
            gameManager.RoundBonusItem();
        }
    }

}