using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TopDown.Shooting;

namespace TopDown.Enemy{//namespace to organize code and avoid naming conflicts
public class Spawner : MonoBehaviour
{
    //evento estatico
    public static UnityEvent onWaveComplete = new UnityEvent();
    //GameObject for each enemy type to spawn
    [SerializeField] private GameObject AlienS; 
    [SerializeField] private GameObject AlienM; 
    [SerializeField] private GameObject AlienL;

    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float pauseBetweenWaves = 1f;
    [SerializeField] private GameObject healPrefab;
    [SerializeField, Range(0f,1f)] private float healSpawn = 0.7f;//cheance que spanwea un heal item 
    [SerializeField] private int totalWaves = 10;//number of waves before game ends

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
            {//spawnea heal item en posicion aleatoria dentro de los mismos limites que los enemigos
                Vector3 spawnPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0f);
                //instancia el heal item sin rotacion
                Instantiate(healPrefab, spawnPos, Quaternion.identity);
            }

            currentWave++;//sube el numero de oleada
            PlayerPrefs.SetInt("CurrentWave", currentWave);//se guarda en player prefs
            enemiesPerWave += 3;//incrementa el numero de enemigos por oleada
        }
    }
    private IEnumerator SpawnWave(int count)
    {
        enemiesAliveInWave = count;
        for (int i = 0; i < count; i++)
        {
            SpawnRandomEnemy();//spawnea enemigo
            yield return new WaitForSeconds(spawnInterval);//espera antes de spawnear siguiente enemigo
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
    public void EnemyDied()
    {
        if (waveInProgress)
        {
            enemiesAliveInWave--;//reducir el numero de enemigos vivos
            if (enemiesAliveInWave <= 0)
            {//si no quedan enemigos vivos
                enemiesAliveInWave = 0;
                onWaveComplete.Invoke();
            }
        }
    }
}}