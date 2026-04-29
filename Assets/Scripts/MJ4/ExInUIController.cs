using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInUIController : MonoBehaviour
    {
        [Header("Corazones Visuales")]
        // Lista que almacena los dibujos de los corazones en pantalla
        public Image[] livesImages; 

        [Header("Indicador de Extra (Opcional)")]
        // Un texto simple para mostrar cuántas vidas invisibles tenemos
        public Text textoVidasExtra; 

        void Start()
        {
            UpdateLives();
        }

        public void UpdateLives()
        {
            // Consultamos cuántas vidas nos quedan
            int currentLives = PlayerPrefs.GetInt("Lives", 3);

            // 1. Revisamos cada corazón de la pantalla uno por uno
            for (int i = 0; i < livesImages.Length; i++)
            {
                // Si el número de este corazón es menor a mis vidas, lo enciendo
                if (i < currentLives)
                {
                    livesImages[i].enabled = true;
                }
                // Si ya perdí esta vida, lo apago
                else
                {
                    livesImages[i].enabled = false;
                }
            }

            // 2. Controlamos el texto de vidas extra
            if (textoVidasExtra != null) // Verificamos si asignaste el texto en Unity
            {
                // Si tenemos más vidas que dibujos en pantalla...
                if (currentLives > livesImages.Length)
                {
                    // Calculamos cuántas "invisibles" tenemos
                    int vidasInvisibles = currentLives - livesImages.Length; 
                    
                    // Mostramos el texto (ej. "+ 2") y lo encendemos
                    textoVidasExtra.text = "+ " + vidasInvisibles;
                    textoVidasExtra.enabled = true;
                }
                else
                {
                    // Si no sobrepasamos el límite, apagamos el texto
                    textoVidasExtra.enabled = false;
                }
            }
        }
    }
}