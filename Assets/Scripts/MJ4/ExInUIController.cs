using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

namespace DefensoresDeSoftware
{
    public class ExInUIController : MonoBehaviour
    {
        public Text timeText;
        public int time = 60; 

        // Lista que almacena los dibujos de los corazones en pantalla
        public Image[] livesImages; 

        void Start()
        {
            UpdateLives();
        }

        public void UpdateLives()
        {
            // Consultamos cuántas vidas nos quedan
            int currentLives = PlayerPrefs.GetInt("Lives", 3);

            // Revisamos cada corazón de la pantalla uno por uno
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
        }
    }
}