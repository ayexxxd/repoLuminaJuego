using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    [Header("Scene Names (must exist in Build Settings)")]
    [SerializeField] private string miniJuego5StartScene = "StartScene";
    [SerializeField] private string miniJuego4StartScene = "ExInInicio";

    // Button function: opens MiniJuego5 start scene
    public void OpenMiniJuego5Start()
    {
        SceneManager.LoadScene(miniJuego5StartScene);
    }

    // Button function: opens MiniJuego4 start scene
    public void OpenMiniJuego4Start()
    {
        SceneManager.LoadScene(miniJuego4StartScene);
    }
}
