﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(SpawnAllWaves());

    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex=startingWave; waveIndex<waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveconfig)
    {
        for (
            int enemyCount = 0; enemyCount <= waveconfig.GetNumberOfEnemies();
            enemyCount++) 
        {
            var newEnemy = Instantiate(waveconfig.GetEnemyPrefab(), waveconfig.GetWaypoints()[0].transform.position, Quaternion.identity);

            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveconfig);

            yield return new WaitForSeconds(waveconfig.GetTimeBetweenSpawn());
        }


    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
