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
                DontDestroyOnLoad(gameObject); 
                // REINICIO DE VIDAS
                PlayerPrefs.SetInt("Lives", initialLives);
            }
            else
            {
                // Si ya existe otro GameControl (ej. al reiniciar nivel), nos destruimos
                Destroy(gameObject);
            }
        }

        // Reemplaza tu función SpendLives() por esta nueva:
        public void TakeDamage(int damageAmount)
        {
            // Leemos las vidas actuales guardadas en memoria
            int currentLives = PlayerPrefs.GetInt("Lives", initialLives);
            
            // Restamos el daño que nos mandó la bala
            currentLives -= damageAmount;

            // Guardamos el nuevo valor
            PlayerPrefs.SetInt("Lives", currentLives);

            // Buscamos a la UI para que actualice los corazones en pantalla de inmediato
            ExInUIController ui = FindAnyObjectByType<ExInUIController>();
            if (ui != null)
            {
                ui.UpdateLives();
            }

            // Si llegamos a 0 vidas, mandamos a la pantalla de Game Over
            if (currentLives <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("ExInDerrota");
            }
        }
    }
}