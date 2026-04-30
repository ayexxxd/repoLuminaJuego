using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //GameObject for each enemy type to spawn
    [SerializeField] private GameObject AlienS; 
    [SerializeField] private GameObject AlienM; 
    [SerializeField] private GameObject AlienL;

    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float pauseBetweenWaves = 5f;
    [Header("Healer Item")]
    [SerializeField] private GameObject healPrefab;
    [SerializeField, Range(0f,1f)] private float healSpawnChance = 0.3f;//1/3 chance by default
    [SerializeField] private int totalWaves = 10;//number of waves before game ends

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
            Debug.Log($"Wave {currentWave}");
            yield return StartCoroutine(SpawnWave(enemiesPerWave));

            Debug.Log($"Wave {currentWave} complete");
            //wait before the next wave
            yield return new WaitForSeconds(pauseBetweenWaves);

            //chance to spawn a healing pickup after the wave
            if (healPrefab != null && Random.value < healSpawnChance)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0f);
                Instantiate(healPrefab, spawnPos, Quaternion.identity);
            }

            currentWave++;
            PlayerPrefs.SetInt("CurrentWave", currentWave);//save current wave to PlayerPrefs for end screen display
            enemiesPerWave += 3; // increase enemies per wave for difficulty
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
        //array of enemty types
        GameObject[] enemies={AlienS,AlienM,AlienL};
        //spawn random enemy at random position within the given x,y,z bounds
        GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];
        //instantiates enemy at random position with no rotation
        Instantiate(randomEnemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
    }
}