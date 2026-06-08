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
                SubmitWord();
            }
        }

        private void OnInputWave(int wave)
        {
            if (panelUI == null)
            {
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
        }

        public void SubmitFromButton()
        {
            PlaySubmitSound();
            SubmitWord();
        }

        private void Update()
        {
            if (panelUI != null && panelUI.activeSelf)
            {
                if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    PlaySubmitSound();
                    SubmitWord();
                }
                if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    ClosePanel();
                }
            }
        }

        private void SubmitWord()
        {
            string word = input != null ? input.text.Trim() : "";
            if (weaponModifier == null)
            {
                weaponModifier = FindAnyObjectByType<WeaponModifier>();
            }
            if (weaponModifier != null && !string.IsNullOrEmpty(word))
            {
                weaponModifier.TryApplyUpgrade(word);
            }
            ClosePanel();
        }

        private void ClosePanel()
        {
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