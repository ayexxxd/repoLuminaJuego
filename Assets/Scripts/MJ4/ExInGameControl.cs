using UnityEngine;
using UnityEngine.SceneManagement; 

namespace DefensoresDeSoftware
{
    public class ExInGameControl : MonoBehaviour
    {
        // Variable global para acceder a este script desde cualquier lugar
        public static ExInGameControl Instance;
        public int initialLives = 3;


        [Header("Límites Globales de Pantalla")]
        public float minX = -8f;
        public float maxX = 8f;
        public float maxY = 5f;  // Ajusta en el Inspector
        public float minY = -5f; // Ajusta en el Inspector

        void Awake()
        {
            // Si no hay un GameControl, nos asignamos como el principal
            if (Instance == null)
            {
                Instance = this;
                // Nos protegemos para no ser destruidos al cambiar de nivel
                DontDestroyOnLoad(gameObject); 
            }
            else
            {
                // Si ya existe otro GameControl (ej. al reiniciar nivel), nos destruimos
                Destroy(gameObject);
            }
        }

        public void SpendLives()
        {
            // Leemos las vidas actuales guardadas en memoria y restamos una
            int currentLives = PlayerPrefs.GetInt("Lives", initialLives);
            currentLives--;

            // Guardamos el nuevo valor
            PlayerPrefs.SetInt("Lives", currentLives);

            // Si llegamos a 0 vidas, mandamos a la pantalla de Game Over
            if (currentLives <= 0)
            {
                SceneManager.LoadScene("ExInEndScene");
            }
        }
    }
}