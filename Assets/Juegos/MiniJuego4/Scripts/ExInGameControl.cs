using UnityEngine;
using UnityEngine.SceneManagement; 

namespace DefensoresDeSoftware
{
    public class ExInGameControl : MonoBehaviour
    {
        // Ajustes generales
        public static ExInGameControl Instance;
        public int initialLives = 3;

        // Ajustes de la pantalla
        [Header("Límites Globales de Pantalla")]
        public float minX = -8f;
        public float maxX = 8f;
        public float maxY = 5f;
        public float minY = -5f;

        // Ajustes de la tienda
        [Header("Costos de Mejoras (en Tokens)")]
        public int costoDamage = 2;
        public int costoVelocidad = 1;
        public int costoVida = 1;

        // Conexión al API
        [Header("API")]
        public string apiBaseUrl = "https://10.22.186.108:5001";
        public int idUsuario = 4;

        // Aseguramos que solo haya una objeto gamecontrol
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                idUsuario = PlayerPrefs.GetInt("userid", 4);
                ResetearProgreso();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void ResetearProgreso()
        {
            PlayerPrefs.SetInt("Lives", initialLives);
            PlayerPrefs.SetInt("BonusDamage", 0);
            PlayerPrefs.SetInt("BonusSpeed", 0);
            PlayerPrefs.SetInt("WhirlpoolTokens", 0);
            PlayerPrefs.SetInt("OleadasCompletadas", 0);
            PlayerPrefs.SetInt("PreguntasCorrectas", 0);
            PlayerPrefs.Save();
        }

        // Controla las vidas y el daño y comunica con el ui
        public void TakeDamage(int damageAmount)
        {
            int currentLives = PlayerPrefs.GetInt("Lives", initialLives);
            currentLives -= damageAmount;
            PlayerPrefs.SetInt("Lives", currentLives);

            ExInUIController ui = FindAnyObjectByType<ExInUIController>();
            if (ui != null) ui.UpdateLives();

            if (currentLives <= 0)
            {
                SceneManager.LoadScene("ExInEndScene");
            }
        }

        public void AddWhirlpoolToken()
        {
            int tokens = PlayerPrefs.GetInt("WhirlpoolTokens", 0);
            tokens++;
            PlayerPrefs.SetInt("WhirlpoolTokens", tokens);

            ExInUIController ui = FindAnyObjectByType<ExInUIController>();
            if (ui != null) ui.UpdateTokens();
        }

        public bool ComprarMejora(TipoMejora mejora)
        {
            int tokens = PlayerPrefs.GetInt("WhirlpoolTokens", 0);
            int costo = ObtenerCosto(mejora);

            if (tokens < costo)
            {
                Debug.Log("Tokens insuficientes para: " + mejora);
                return false;
            }

            tokens -= costo;
            PlayerPrefs.SetInt("WhirlpoolTokens", tokens);
            PlayerPrefs.Save();

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

                    ExInUIController ui = FindAnyObjectByType<ExInUIController>();
                    if (ui != null) ui.UpdateLives();
                    break;
            }

            PlayerPrefs.Save();
            Debug.Log("Mejora comprada: " + mejora + " | Tokens restantes: " + tokens);

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

        public void ReportarOleadasCompletadas(int total)
        {
            PlayerPrefs.SetInt("OleadasCompletadas", total);
            PlayerPrefs.Save();
        }

        public void RegistrarRespuestaCorrecta()
        {
            int correctas = PlayerPrefs.GetInt("PreguntasCorrectas", 0);
            PlayerPrefs.SetInt("PreguntasCorrectas", correctas + 1);
            PlayerPrefs.Save();
        }
    }

    public enum TipoMejora { Dano, Velocidad, Vida }
}
