using UnityEngine;
using TMPro;
using UnityEngine.Events;
using TopDown.Enemy;
using System.Collections;

namespace TopDown.Shooting
{
    public class WordInputPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField input;//referencia
        [SerializeField] private GameObject panelUI;//

        public UnityEvent<string> onValidWord;

        private void Awake()
        {
            Spawner.onWaveComplete.AddListener(OnWaveComplete);
        }

        private void Start()
        {
            panelUI.SetActive(false);
        }
        private void OnWaveComplete()
        {
            panelUI.SetActive(true);
        }
}}