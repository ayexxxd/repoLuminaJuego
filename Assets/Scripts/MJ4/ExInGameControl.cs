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
        public float maxY = 5f;
        public float minY = -5f;

        // ==========================================
        //        COSTOS DE MEJORAS (Editables)
        // ==========================================
        [Header("Costos de Mejoras (en Tokens)")]
        public int costoDamage = 2;
        public int costoVelocidad = 1;
        public int costoVida = 3;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); 
                PlayerPrefs.SetInt("Lives", initialLives);
                // Reseteamos las mejoras al iniciar una partida nueva
                PlayerPrefs.SetInt("BonusDamage", 0);
                PlayerPrefs.SetInt("BonusSpeed", 0);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void TakeDamage(int damageAmount)
        {
            int currentLives = PlayerPrefs.GetInt("Lives", initialLives);
            currentLives -= damageAmount;
            PlayerPrefs.SetInt("Lives", currentLives);

            ExInUIController ui = FindAnyObjectByType<ExInUIController>();
            if (ui != null) ui.UpdateLives();

            if (currentLives <= 0)
            {
                SceneManager.LoadScene("ExInDerrota");
            }
        }

        public void AddWhirlpoolToken()
        {
            int tokens = PlayerPrefs.GetInt("WhirlpoolTokens", 0);
            tokens++;
            PlayerPrefs.SetInt("WhirlpoolTokens", tokens);
            PlayerPrefs.Save();

            ExInUIController ui = FindAnyObjectByType<ExInUIController>();
            if (ui != null) ui.UpdateTokens();
        }

        // ==========================================
        //        SISTEMA DE MEJORAS
        // ==========================================

        /// <summary>
        /// Intenta comprar la mejora solicitada. Devuelve true si la compra fue exitosa.
        /// </summary>
        public bool ComprarMejora(TipoMejora mejora)
        {
            int tokens = PlayerPrefs.GetInt("WhirlpoolTokens", 0);
            int costo = ObtenerCosto(mejora);

            if (tokens < costo)
            {
                Debug.Log("Tokens insuficientes para: " + mejora);
                return false;
            }

            // Descontamos los tokens
            tokens -= costo;
            PlayerPrefs.SetInt("WhirlpoolTokens", tokens);
            PlayerPrefs.Save();

            // Aplicamos la mejora
            switch (mejora)
            {
                case TipoMejora.Dano:
                    int bonusDmg = PlayerPrefs.GetInt("BonusDamage", 0);
                    PlayerPrefs.SetInt("BonusDamage", bonusDmg + 1);
                    break;

                case TipoMejora.Velocidad:
                    int bonusSpd = PlayerPrefs.GetInt("BonusSpeed", 0);
                    PlayerPrefs.SetInt("BonusSpeed", bonusSpd + 1);
                    break;

                case TipoMejora.Vida:
                    int currentLives = PlayerPrefs.GetInt("Lives", initialLives);
                    PlayerPrefs.SetInt("Lives", currentLives + 1);
                    // Actualizamos el HUD de vidas
                    ExInUIController ui = FindAnyObjectByType<ExInUIController>();
                    if (ui != null) ui.UpdateLives();
                    break;
            }

            PlayerPrefs.Save();
            Debug.Log("Mejora comprada: " + mejora + " | Tokens restantes: " + tokens);

            // Notificamos al jugador para que recargue sus stats
            ExInPlayerControl player = FindAnyObjectByType<ExInPlayerControl>();
            if (player != null) player.AplicarMejoras();

            return true;
        }

        public int ObtenerCosto(TipoMejora mejora)
        {
            switch (mejora)
            {
                case TipoMejora.Dano:      return costoDamage;
                case TipoMejora.Velocidad: return costoVelocidad;
                case TipoMejora.Vida:      return costoVida;
                default:                   return 999;
            }
        }
    }

    public enum TipoMejora { Dano, Velocidad, Vida }
}