using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using TMPro; // <-- 1. Agregamos esta línea para que Unity entienda qué es TextMeshPro

namespace DefensoresDeSoftware
{
    public class ExInUIController : MonoBehaviour
    {
        [Header("Corazones Visuales")]
        public Image[] livesImages; 

        [Header("Indicador de Extra (Opcional)")]
        // 2. Cambiamos 'Text' por 'TextMeshProUGUI'
        public TextMeshProUGUI textoVidasExtra; 

        void Start()
        {
            UpdateLives();
        }

        public void UpdateLives()
        {
            int currentLives = PlayerPrefs.GetInt("Lives", 3);

            for (int i = 0; i < livesImages.Length; i++)
            {
                if (i < currentLives)
                {
                    livesImages[i].enabled = true;
                }
                else
                {
                    livesImages[i].enabled = false;
                }
            }

            // 3. El resto del código funciona exactamente igual
            if (textoVidasExtra != null) 
            {
                if (currentLives > livesImages.Length)
                {
                    int vidasInvisibles = currentLives - livesImages.Length; 
                    
                    textoVidasExtra.text = "+ " + vidasInvisibles;
                    textoVidasExtra.enabled = true;
                }
                else
                {
                    textoVidasExtra.enabled = false;
                }
            }
        }
    }
}