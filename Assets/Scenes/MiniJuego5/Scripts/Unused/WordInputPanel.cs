using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using TopDown.Enemy;
using System.Collections;
using UnityEngine.InputSystem;

namespace TopDown.Shooting
{
    public class WordInputPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField input;//referencia
        [SerializeField] private Button submitButton;
        [SerializeField] private Button clearButton;
        [SerializeField] private GameObject panelUI;//
        [SerializeField] private bool clearInputAfterSubmit = true;
        [SerializeField] private Key submitKey = Key.Enter;
        [SerializeField] private Key clearKey = Key.Escape;

        public UnityEvent<string> onWordSubmitted;

        private void Awake()
        {
            Spawner.onWaveComplete.AddListener(OnWaveComplete);
            if (submitButton != null)
            {
                submitButton.onClick.AddListener(SubmitCurrentWord);
            }
            if (clearButton != null)
            {
                clearButton.onClick.AddListener(ClearCurrentWord);
            }
        }

        private void OnDestroy()
        {
            Spawner.onWaveComplete.RemoveListener(OnWaveComplete);

            if (submitButton != null)
            {
                submitButton.onClick.RemoveListener(SubmitCurrentWord);
            }

            if (clearButton != null)
            {
                clearButton.onClick.RemoveListener(ClearCurrentWord);
            }
        }

        private void Start()
        {
            panelUI.SetActive(false);
            SetControlsVisible(false);
        }

        private void Update()
        {
            if (panelUI == null || !panelUI.activeSelf)
            {
                return;
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return;
            }

            if (keyboard[submitKey].wasPressedThisFrame)
            {
                SubmitCurrentWord();
            }

            if (keyboard[clearKey].wasPressedThisFrame)
            {
                ClearCurrentWord();
            }
        }

        private void OnWaveComplete()
        {
            panelUI.SetActive(true);
            SetControlsVisible(true);
            if (input != null)
            {
                input.text = string.Empty;
                input.ActivateInputField();
                input.Select();
            }
        }

        public void HidePanel()
        {
            if (panelUI != null)
            {
                panelUI.SetActive(false);
            }

            SetControlsVisible(false);
        }

        public void SubmitCurrentWord()
        {
            if (input == null)
            {
                return;
            }

            SubmitWord(input.text);
        }

        public void ClearCurrentWord()
        {
            if (input == null)
            {
                return;
            }

            input.text = string.Empty;
            input.ActivateInputField();
            input.Select();
        }

        public void SubmitWord(string submittedText)
        {
            string normalizedWord = NormalizeWord(submittedText);
            if (string.IsNullOrEmpty(normalizedWord))
            {
                return;
            }

            onWordSubmitted?.Invoke(normalizedWord);

            if (clearInputAfterSubmit && input != null)
            {
                input.text = string.Empty;
                input.ActivateInputField();
                input.Select();
            }
        }

        private string NormalizeWord(string submittedText)
        {
            return (submittedText ?? string.Empty).Trim().ToLowerInvariant();
        }

        private void SetControlsVisible(bool visible)
        {
            if (submitButton != null)
            {
                submitButton.gameObject.SetActive(visible);
            }

            if (clearButton != null)
            {
                clearButton.gameObject.SetActive(visible);
            }
        }
}}

