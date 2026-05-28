using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace DefensoresDeSoftware
{
    public enum CategoriaSpawn 
    { 
        General,
        Flanco,
        Especial,
        Jefe
    }

    [System.Serializable]
    public class EnemigoConfig
    {
        public GameObject prefab;
        public int costo = 1; 
        public int oleadaMinima = 1; 
        public CategoriaSpawn categoriaRequerida = CategoriaSpawn.General; 
    }

    [System.Serializable]
    public class PuntoSpawnConfig
    {
        public Transform punto; 
        public CategoriaSpawn categoriaDelPunto = CategoriaSpawn.General; 
        
        [Header("Rango de Varianza")]
        public Vector2 varianzaMin = new Vector2(-1f, -1f); 
        public Vector2 varianzaMax = new Vector2(1f, 1f);
    }

    [System.Serializable]
    public class OleadaConfig
    {
        public int presupuesto; 
        public GameObject enemigoForzado; 
        public CategoriaSpawn categoriaDelBoss = CategoriaSpawn.Jefe; 
        public int enemigosPorGrupo = 2; 
    }

    public class ExInSpawner : MonoBehaviour
    {
        [Header("Qué y Dónde")]
        public EnemigoConfig[] enemigosDisponibles; 
        public PuntoSpawnConfig[] spawnPoints; 

        [Header("Configuración de Oleadas")]
        public int currentWave = 1;
        public OleadaConfig[] configuracionOleadas; 
        
        [Header("Tiempos y Dificultad")]
        public float timeBetweenSpawns = 2f; 
        public float timeBetweenWaves = 6f; 
        public float speedUpPerWave = 0.2f; 

        private bool isSpawning = false;

        private bool oleadaSaltada = false;

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

        public void SaltarOleadaActual()
        {
            if (!isSpawning) return;

            oleadaSaltada = true;
            Debug.Log("[Spawner] Oleada saltada → avanzando a la siguiente.");
        }

        IEnumerator SpawnWave()
        {
            ExInUIController ui = FindAnyObjectByType<ExInUIController>();
            
            if (ui != null) ui.ActualizarNivel(currentWave);

            isSpawning = true;
            oleadaSaltada = false;

            OleadaConfig oleadaActual = configuracionOleadas[currentWave - 1];
            int presupuestoRestante = oleadaActual.presupuesto;

            if (oleadaActual.enemigoForzado != null)
            {
                Vector2 posicionFinal = CalcularPosicionConVarianza(oleadaActual.categoriaDelBoss);
                Instantiate(oleadaActual.enemigoForzado, posicionFinal, Quaternion.identity);
                yield return new WaitForSeconds(timeBetweenSpawns * 2f); 
            }

            int conteoGrupoActual = 0; 

            while (presupuestoRestante > 0 && !oleadaSaltada)
            {
                List<EnemigoConfig> tienda = new List<EnemigoConfig>();
                foreach (EnemigoConfig enemigo in enemigosDisponibles)
                {
                    if (currentWave >= enemigo.oleadaMinima && presupuestoRestante >= enemigo.costo)
                        tienda.Add(enemigo);
                }

                if (tienda.Count == 0) break; 

                EnemigoConfig enemigoComprado = tienda[Random.Range(0, tienda.Count)];
                presupuestoRestante -= enemigoComprado.costo;

                Vector2 posicionFinal = CalcularPosicionConVarianza(enemigoComprado.categoriaRequerida);
                Instantiate(enemigoComprado.prefab, posicionFinal, Quaternion.identity);

                conteoGrupoActual++; 

                if (conteoGrupoActual >= oleadaActual.enemigosPorGrupo) 
                {
                    yield return new WaitForSeconds(timeBetweenSpawns);
                    conteoGrupoActual = 0; 
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            if (ui != null)
            {
                ui.MostrarTrivia();

                yield return new WaitUntil(() => ui.triviaFinalizada);
            }

            timeBetweenSpawns = Mathf.Max(0.5f, timeBetweenSpawns - speedUpPerWave); 

            if (currentWave == configuracionOleadas.Length)
            {
                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
                // Reportar antes de que el spawner sea destruido por el cambio de escena
                if (ExInGameControl.Instance != null)
                    ExInGameControl.Instance.ReportarOleadasCompletadas(currentWave);
                Debug.Log("¡VICTORIA TOTAL!");
                isSpawning = false;
                SceneManager.LoadScene("ExInEndScene");
                yield break;
            }

            if (ExInGameControl.Instance != null)
                ExInGameControl.Instance.ReportarOleadasCompletadas(currentWave);

            currentWave++;
            isSpawning = false;
            oleadaSaltada = false;
            yield return new WaitForSeconds(timeBetweenWaves);
            StartNextWave();
        }

        private Vector2 CalcularPosicionConVarianza(CategoriaSpawn categoriaBuscada)
        {
            List<PuntoSpawnConfig> puntosValidos = new List<PuntoSpawnConfig>();
            
            foreach (PuntoSpawnConfig sp in spawnPoints)
            {
                if (sp.categoriaDelPunto == categoriaBuscada)
                    puntosValidos.Add(sp);
            }

            if (puntosValidos.Count == 0)
            {
                Debug.LogWarning("Falta un punto de spawn para la categoría: " + categoriaBuscada + ". Usando uno al azar.");
                puntosValidos.AddRange(spawnPoints);
            }

            PuntoSpawnConfig configElegida = puntosValidos[Random.Range(0, puntosValidos.Count)];
            
            if (configElegida.punto == null) return Vector2.zero;

            float offsetX = Random.Range(configElegida.varianzaMin.x, configElegida.varianzaMax.x);
            float offsetY = Random.Range(configElegida.varianzaMin.y, configElegida.varianzaMax.y);

            return (Vector2)configElegida.punto.position + new Vector2(offsetX, offsetY);
        }

        void OnDrawGizmos()
        {
            if (spawnPoints == null) return;
            foreach (PuntoSpawnConfig sp in spawnPoints)
            {
                if (sp.punto != null)
                {
                    Gizmos.color = new Color(1f, 0f, 0f, 0.3f); 
                    Vector2 centro = (Vector2)sp.punto.position + (sp.varianzaMin + sp.varianzaMax) / 2f;
                    Vector2 tamano = new Vector2(Mathf.Abs(sp.varianzaMax.x - sp.varianzaMin.x), Mathf.Abs(sp.varianzaMax.y - sp.varianzaMin.y));
                    Gizmos.DrawCube(centro, tamano);
                }
            }
        }
    }
}