using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TopDown.Shooting;

namespace TopDown.Enemy{//namespace to organize code and avoid naming conflicts
public class Spawner : MonoBehaviour
{
    // Static event that fires when a wave is complete - anyone can listen
    public static UnityEvent onWaveComplete = new UnityEvent();
    //GameObject for each enemy type to spawn
    [SerializeField] private GameObject AlienS; 
    [SerializeField] private GameObject AlienM; 
    [SerializeField] private GameObject AlienL;

    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float pauseBetweenWaves = 5f;
    [Header("Healer Item")]
    [SerializeField] private GameObject healPrefab;
    [SerializeField, Range(0f,1f)] private float healSpawnChance = 0.5f;//1/3 chance by default
    [SerializeField] private int totalWaves = 10;//number of waves before game ends

    private int currentWave = 1;
    public int CurrentWave => currentWave;
    public int TotalWaves => totalWaves;
    
    private int enemiesAliveInWave = 0;
    private bool waveInProgress = false;

    void Start()
    {
        Debug.Log($"Spawner started. onWaveComplete listeners: {onWaveComplete.GetPersistentEventCount()}");
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (currentWave <= totalWaves)
        {
            //Debug.Log($"Wave {currentWave}");
            waveInProgress = true;
            yield return StartCoroutine(SpawnWave(enemiesPerWave));
            //Debug.Log($"Wave {currentWave} complete");
            
            // Wait for all enemies in this wave to be killed
            while (enemiesAliveInWave > 0)
            {
                yield return new WaitForSeconds(0.5f); // Check every half second
            }
            
            Debug.Log("Wave complete! Firing onWaveComplete event.");
            onWaveComplete?.Invoke(); // Fire the event - WordInputPanel listens to this
            waveInProgress = false;
            
            //wait before the next wave
            yield return new WaitForSeconds(pauseBetweenWaves);

            //chance to spawn a healing pickup after the wave
            if (Random.value < healSpawnChance)
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
        enemiesAliveInWave = count; // Track enemies for this wave
        Debug.Log($"Spawning {count} enemies for wave {currentWave}");
        for (int i = 0; i < count; i++)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    /// <summary>
    /// Called by EnemyHealth when an enemy dies. Tracks wave completion.
    /// </summary>
    public void EnemyDied()
    {
        if (waveInProgress)
        {
            enemiesAliveInWave--;
            Debug.Log($"Enemy died! Enemies remaining in wave: {enemiesAliveInWave}");
            if (enemiesAliveInWave <= 0)
            {
                enemiesAliveInWave = 0; // Prevent negative values
                Debug.Log("All enemies defeated! Wave complete.");
            }
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

}}