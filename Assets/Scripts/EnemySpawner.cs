using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    int startingWave = 0;
    // Start is called before the first frame update
    void Start()
    {
        var currentWave = waveConfigs[startingWave];
        StartCoroutine(SpawnAllEnemiesInWave(currentWave));

    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveconfig)
    {
        for (
            int enemyCount = 0; 
            enemyCount <= waveconfig.GetNumberOfEnemies();
            enemyCount++) 
        {
            Instantiate(waveconfig.GetEnemyPrefab(), waveconfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(
                waveconfig.GetTimeBetweenSpawn()
                );
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
