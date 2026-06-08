using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace TopDown.Enemy
{
public class Spawner : MonoBehaviour
{
    public static UnityEvent<int> onInputWave = new UnityEvent<int>();
    public static bool waitingForInput = false;
    //GameObject for each enemy type to spawn
    [SerializeField] private GameObject AlienS; 
    [SerializeField] private GameObject AlienM; 
    [SerializeField] private GameObject AlienL;

    [SerializeField] private float spawnInterval = 0.6f;
    [SerializeField] private int enemiesPerWave = 10;
    [SerializeField] private float pauseBetweenWaves = 0.5f;
    [SerializeField] private GameObject healPrefab;
    [SerializeField, Range(0f,1f)] private float healSpawn = 0.7f;//cheance que spanwea un heal item
    [SerializeField] private int totalWaves = 10;//number of waves before game ends
    [SerializeField] private int spawnClusterSize = 2;//number of enemies spawned per interval
    
    [SerializeField] private float minSpawnDistanceFromPlayer = 3f;
    [SerializeField] private float spawnEdgeMargin = 0.5f;
    [SerializeField] private Vector2 gridMinBounds = new Vector2(-15f, -10f);
    [SerializeField] private Vector2 gridMaxBounds = new Vector2(15f, 10f);

    private int currentWave = 1;//track de oleada actual
    public int CurrentWave => currentWave;//propiedad para acceder a la oleada actual
    public int TotalWaves => totalWaves;//propiedad para acceder al total de oleadas
    
    private int enemiesAliveInWave = 0;//track de enemigos vivos en la oleada actual
    private bool waveInProgress = false;//track si una oleada esta en progreso

    void Start()
    {//iniciar la rutina de oleadas
        StartCoroutine(WaveLoop());
        PlayerPrefs.SetInt("CurrentWave", 1);//guardar oleada actual en player prefs
    }

    private IEnumerator WaveLoop()
    {
        while (currentWave <= totalWaves)//mientras no se hayan completado todas las oleadas
        {
            waveInProgress = true;
            
            // Show input panel on even waves (2,4,6,8,10)
            if (currentWave % 2 == 0)
            {
                waitingForInput = true;
                onInputWave.Invoke(currentWave);
                float waitStart = Time.realtimeSinceStartup;
                yield return new WaitUntil(() => !waitingForInput || Time.realtimeSinceStartup - waitStart > 8f);
                waitingForInput = false;
            }
            
            yield return StartCoroutine(SpawnWave(enemiesPerWave));
            
            while (enemiesAliveInWave > 0)//espera a que todos los enemigos de la oleada mueran
            {
                yield return new WaitForSeconds(0.5f);//revisar cada medio segundo
            }
            waveInProgress = false;
            
            //esperar antes de siguiente oleada
            yield return new WaitForSeconds(pauseBetweenWaves);



            //puede aparecer un heal item
            if (Random.value < healSpawn)
            {
                Vector3 spawnPos = GetSpawnPositionAtCameraEdge();
                Instantiate(healPrefab, spawnPos, Quaternion.identity);
            }

            currentWave++;//sube el numero de oleada
            PlayerPrefs.SetInt("CurrentWave", currentWave);//se guarda en player prefs
            enemiesPerWave += 5;//incrementa el numero de enemigos por oleada
        }
    }
    private IEnumerator SpawnWave(int count)
    {
        enemiesAliveInWave = count;
        int spawned = 0;
        while (spawned < count)
        {
            int batch = Mathf.Min(spawnClusterSize, count - spawned);
            for (int b = 0; b < batch; b++)
            {
                SpawnRandomEnemy();
                spawned++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void SpawnRandomEnemy()
    {
        GameObject[] enemies = { AlienS, AlienM, AlienL };
        GameObject randomEnemy = enemies[Random.Range(0, enemies.Length)];

        Vector3 spawnPos = GetSpawnPositionAtCameraEdge();
        Instantiate(randomEnemy, spawnPos, Quaternion.identity);
    }

    private Vector3 GetSpawnPositionAtCameraEdge()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return new Vector3(Random.Range(-3f, 3f), Random.Range(-3.5f, 3.5f), 0f);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerPos = player != null ? player.transform.position : Vector3.zero;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;
        float halfW = camWidth + spawnEdgeMargin;
        float halfH = camHeight + spawnEdgeMargin;

        Vector3 candidate = Vector3.zero;
        int attempts = 0;
        int maxAttempts = 20;

        while (attempts < maxAttempts)
        {
            int edge = Random.Range(0, 4);
            float x = 0f, y = 0f;

            switch (edge)
            {
                case 0: // top
                    x = Random.Range(-halfW, halfW);
                    y = halfH;
                    break;
                case 1: // bottom
                    x = Random.Range(-halfW, halfW);
                    y = -halfH;
                    break;
                case 2: // left
                    x = -halfW;
                    y = Random.Range(-halfH, halfH);
                    break;
                case 3: // right
                    x = halfW;
                    y = Random.Range(-halfH, halfH);
                    break;
            }

            candidate = new Vector3(playerPos.x + x, playerPos.y + y, 0f);

            // Clamp to grid bounds
            candidate.x = Mathf.Clamp(candidate.x, gridMinBounds.x, gridMaxBounds.x);
            candidate.y = Mathf.Clamp(candidate.y, gridMinBounds.y, gridMaxBounds.y);

            // Ensure not too close to player
            float dist = Vector3.Distance(candidate, playerPos);
            if (dist >= minSpawnDistanceFromPlayer)
            {
                return candidate;
            }

            attempts++;
        }

        // Fallback: force position further out on random edge
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 fallback = playerPos + new Vector3(Mathf.Cos(angle) * minSpawnDistanceFromPlayer, Mathf.Sin(angle) * minSpawnDistanceFromPlayer, 0f);
        fallback.x = Mathf.Clamp(fallback.x, gridMinBounds.x, gridMaxBounds.x);
        fallback.y = Mathf.Clamp(fallback.y, gridMinBounds.y, gridMaxBounds.y);
        return fallback;
    }
    public void EnemyDied()
    {
        if (waveInProgress)
        {
            enemiesAliveInWave--;//reducir el numero de enemigos vivos
            if (enemiesAliveInWave <= 0)
            {
                enemiesAliveInWave = 0;
            }
        }
    }
}}