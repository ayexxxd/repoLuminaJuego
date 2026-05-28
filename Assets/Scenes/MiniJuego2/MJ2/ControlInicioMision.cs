using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class ControlInicioMision : MonoBehaviour
{


// muestra los tokens acumulados y este agarra el texto puesto en unity para reflehar e lresultado
    public TMP_Text textoTokensFinales;

// con este le digo que está trabajando con el usuario numero 1 de la base de datos
    public int idUsuario = 1;




// en cuanto el usuario entre a EMI aparezcan los tokens acumulados de todas las partidas
    void Start()
    {
        StartCoroutine(CargarTokensDesdeApi());
    }
// /////////////


// /////////////
    IEnumerator CargarTokensDesdeApi()
    {
        string url = "http://127.0.0.1:5000/mision/tokens/" + idUsuario;

        UnityWebRequest request = UnityWebRequest.Get(url); // este es lo que permite que Unity pida info al API

        yield return request.SendWebRequest(); // esto a la web 

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            UsuarioTokens usuario = JsonUtility.FromJson<UsuarioTokens>(json);

            textoTokensFinales.text = usuario.WhirlTokens.ToString();
        }
        else
        {
            textoTokensFinales.text = "0";

            Debug.Log("Error al cargar tokens: " + request.error);
        }
    }
// /////////////



    public void IniciarJuego() // boton iniciar juego
    {
        DatosJuego.ReiniciarPartida();  // reinicia el nivel actual, cada que inicies desde EMI el juego empieza en una partida limpia de registro

        SceneManager.LoadScene("EN1");
    }

    public void RegresarMenuPrincipal() // boton menu
    {
        SceneManager.LoadScene("MenuScene"); // todos los mini juegos
    }
}


[System.Serializable]
public class UsuarioTokens
{
    public int IdUsuario;
    public string Nombre;
    public int WhirlTokens;
}

// el monde que usar Unity para leer la respuesta del API, o sea como los mismos campos que devuelve JSON 