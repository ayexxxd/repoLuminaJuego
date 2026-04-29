using UnityEngine;
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInSpawner : MonoBehaviour
    {
        public GameObject[] enemyPrefabs; 
        public Transform[] spawnPoints; 

        public int currentWave = 1;
        
        // Convertimos el número suelto en una lista de números.
        // En Unity, podrás agregar elementos. Ej: Element 0 = 5, Element 1 = 8.
        public int[] enemiesPerWave; 
        
        public float timeBetweenSpawns = 2f;
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
            // CANDADO DE SEGURIDAD: Comprobamos si la oleada actual existe en nuestra lista.
            // Si currentWave es mayor al tamaño de nuestro arreglo, significa que el jugador ganó.
            if (currentWave > enemiesPerWave.Length)
            {
                Debug.Log("¡Todas las oleadas completadas! No hay más enemigos.");
                // yield break funciona como un "return", aborta la corrutina inmediatamente.
                yield break; 
            }

            isSpawning = true;
            Debug.Log("Iniciando Oleada " + currentWave);

            // TRADUCCIÓN DE ÍNDICES: Restamos 1 a la oleada actual. 
            // Si es la Oleada 1, buscará el valor en la posición 0 del arreglo.
            int enemiesThisWave = enemiesPerWave[currentWave - 1];

            // Nuestro ciclo for ahora usa el límite exacto que configuraste para esta ronda
            for (int i = 0; i < enemiesThisWave; i++)
            {
                GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);

                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            // Terminó de lanzar los enemigos de esta ronda
            isSpawning = false;
            
            // Preparamos el reloj interno para la siguiente ronda, 
            // pero ya no sumamos enemigos matemáticamente.
            currentWave++;
        }
    }
}