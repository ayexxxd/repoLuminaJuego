using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using TopDown.Enemy;

namespace TopDown.Shooting
{
    public class WordInputPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField input;
        [SerializeField] private GameObject panelUI;
        [SerializeField] private WeaponModifier weaponModifier;
        [SerializeField] private AudioClip submitSFX;
        private AudioSource audioSource;

        private void Awake()
        {
            Spawner.onInputWave.AddListener(OnInputWave);
        }

        private void OnDestroy()
        {
            Spawner.onInputWave.RemoveListener(OnInputWave);
        }

        private void Start()
        {
            if (panelUI != null)
                panelUI.SetActive(false);
            if (input != null)
                input.onEndEdit.AddListener(OnInputEndEdit);
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        private void OnInputEndEdit(string text)
        {
            if (panelUI != null && panelUI.activeSelf)
            {
                Debug.Log("WordInputPanel: onEndEdit triggered with text: '" + text + "'");
                SubmitWord();
            }
        }

        private void OnInputWave(int wave)
        {
            Debug.Log("WordInputPanel: Received input wave " + wave);
            if (panelUI == null)
            {
                Debug.LogError("WordInputPanel: panelUI is null! Cannot show panel.");
                Spawner.waitingForInput = false;
                return;
            }
            panelUI.SetActive(true);
            Time.timeScale = 0f;
            if (input != null)
            {
                input.text = "";
                input.Select();
                input.ActivateInputField();
            }
            Debug.Log("WordInputPanel: Panel shown. Type a word and press Enter or click Submit.");
        }

        public void SubmitFromButton()
        {
            Debug.Log("WordInputPanel: Submit button clicked.");
            PlaySubmitSound();
            SubmitWord();
        }

        private void Update()
        {
            if (panelUI != null && panelUI.activeSelf)
            {
                if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    Debug.Log("WordInputPanel: Enter key pressed.");
                    PlaySubmitSound();
                    SubmitWord();
                }
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    Debug.Log("WordInputPanel: Escape pressed — skipping panel.");
                    ClosePanel();
                }
            }
        }

        private void SubmitWord()
        {
            string word = input != null ? input.text.Trim() : "";
            Debug.Log("WordInputPanel: SubmitWord called with word: '" + word + "'");
            if (weaponModifier == null)
            {
                Debug.Log("WordInputPanel: weaponModifier is null, searching scene...");
                weaponModifier = FindAnyObjectByType<WeaponModifier>();
            }
            if (weaponModifier == null)
            {
                Debug.LogError("WordInputPanel: Could not find WeaponModifier in scene!");
            }
            else if (!string.IsNullOrEmpty(word))
            {
                weaponModifier.TryApplyUpgrade(word);
            }
            else
            {
                Debug.Log("WordInputPanel: Word is empty, skipping upgrade.");
            }
            ClosePanel();
        }

        private void ClosePanel()
        {
            Debug.Log("WordInputPanel: Closing panel.");
            if (panelUI != null)
                panelUI.SetActive(false);
            Time.timeScale = 1f;
            Spawner.waitingForInput = false;
        }

        private void PlaySubmitSound()
        {
            if (submitSFX != null && audioSource != null)
                audioSource.PlayOneShot(submitSFX);
        }
    }
}