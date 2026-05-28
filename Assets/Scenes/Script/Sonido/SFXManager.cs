using UnityEngine;

namespace Ximena.Sonido{
    public class SFXManager : MonoBehaviour
    {
        public static SFXManager instancia;

        public AudioClip estrella;
        public AudioClip manchas;
        public AudioClip derrota;       // minúscula
        public AudioClip menuPrincipal; // minúscula
        public AudioClip sceneCarro;    // minúscula
        public AudioClip victoria;      // minúscula

        void Awake()
        {
            if (instancia == null)
            {
                instancia = this;
            }
            else if (instancia != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void Estrella()
        {
            if (estrella == null)
            {
                Debug.LogWarning("SFXManager: clip de estrella no asignado.");
                return;
            }
            AudioSource.PlayClipAtPoint(estrella, Camera.main.transform.position);
        }

        public void Mancha()
        {
            if (manchas == null)
            {
                Debug.LogWarning("SFXManager: clip de manchas no asignado.");
                return;
            }
            AudioSource.PlayClipAtPoint(manchas, Camera.main.transform.position);
        }

        public void GameOver()
        {
            if (derrota == null)
            {
                Debug.LogWarning("SFXManager: clip de derrota no asignado.");
                return;
            }
            AudioSource.PlayClipAtPoint(derrota, Camera.main.transform.position);
        }

        public void MenuMusica()
        {
            AudioSource.PlayClipAtPoint(menuPrincipal, Camera.main.transform.position);
        }

        public void PistaMusica()
        {
            AudioSource.PlayClipAtPoint(sceneCarro, Camera.main.transform.position);
        }

        public void Victoria()
        {
            if (victoria == null)
            {
                Debug.LogWarning("SFXManager: clip de victoria no asignado.");
                return;
            }
            AudioSource.PlayClipAtPoint(victoria, Camera.main.transform.position);
        }
    }
}