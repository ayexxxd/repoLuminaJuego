using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject AlienS; 
    [SerializeField] private GameObject AlienM; 
    [SerializeField] private GameObject AlienL;

    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float pauseBetweenWaves = 5f;
    [SerializeField] private int totalWaves = 10;

    private int currentWave = 1;
    public int CurrentWave => currentWave;
    public int TotalWaves => totalWaves;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (currentWave <= totalWaves)
        {
            Debug.Log($"Wave {currentWave} started!");
            yield return StartCoroutine(SpawnWave(enemiesPerWave));
            
            Debug.Log($"Wave {currentWave} complete. Next wave in {pauseBetweenWaves}s");
            yield return new WaitForSeconds(pauseBetweenWaves);
            
            currentWave++;
        }
    }

    private IEnumerator SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomEnemy()
    {
        GameObject[] enemies = { AlienS, AlienM, AlienL };
        GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
        Instantiate(randomEnemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
    }
}