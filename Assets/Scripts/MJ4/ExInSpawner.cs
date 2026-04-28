using UnityEngine;
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInSpawner : MonoBehaviour
    {
        // Lista de enemigos que podemos generar
        public GameObject[] enemyPrefabs; 
        
        // Puntos invisibles en el mapa por donde saldrán los enemigos
        public Transform[] spawnPoints; 

        public int currentWave = 1;
        public int enemiesPerWave = 5;
        public float timeBetweenSpawns = 2f;
        
        // Candado para evitar que se inicien dos oleadas al mismo tiempo
        private bool isSpawning = false;

        void Start()
        {
            StartNextWave();
        }

        public void StartNextWave()
        {
            if (!isSpawning)
            {
                StartCoroutine(SpawnWave());
            }
        }

        IEnumerator SpawnWave()
        {
            isSpawning = true;
            Debug.Log("Iniciando Oleada " + currentWave);

            // Ciclo que se repite por cada enemigo de la oleada actual
            for (int i = 0; i < enemiesPerWave; i++)
            {
                // Elegimos un enemigo y una posición al azar de nuestras listas
                GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                // Creamos al enemigo en la escena
                Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);

                // Esperamos un momento antes de lanzar al siguiente
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            // Terminó la oleada, quitamos el candado
            isSpawning = false;
            
            // Preparamos los números para que la siguiente oleada tenga más enemigos
            currentWave++;
            enemiesPerWave += 2; 
        }
    }
}