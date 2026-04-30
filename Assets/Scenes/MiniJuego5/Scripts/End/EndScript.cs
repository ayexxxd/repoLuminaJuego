using UnityEngine;
using TMPro;
using UnityEditor.Search;
using UnityEngine.SceneManagement;
namespace TopDown.Enemy{//namespace para evitar conflictos de nombres

public class EndScript : MonoBehaviour
{
    [SerializeField] private AudioClip endSFX;
    [SerializeField] private TextMeshProUGUI scoreText;//reference to score text UI element
    [SerializeField] private TextMeshProUGUI gameOverText;//reference to game over text UI element
    [SerializeField] private TextMeshProUGUI waveText;//reference to wave text UI element

    public void Retry()
    {//funcion del boton de jugar
        SceneManager.LoadScene("ShooterScene");
    }
    public void Quit()
    {//funcion del boton de jugar
        SceneManager.LoadScene("StartScene");
    }
    void Start()
    {   
        GetComponent<AudioSource>().PlayOneShot(endSFX);
        scoreText.text = "Puntaje Final: " + ScoreManager.instance.GetScore(); 

        waveText.text = "Oleada Final: " + PlayerPrefs.GetInt("CurrentWave");
        //retirar el numero de oleada final del PlayerPrefs

        if(PlayerPrefs.GetInt("CurrentWave") > 10)
            gameOverText.text = "¡Has Ganado!";//imprimir ganaste solamente si llegas a la oleada 10
}}
}