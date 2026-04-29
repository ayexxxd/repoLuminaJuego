using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WordInputPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Validation")]
    [SerializeField] private List<string> validWords = new List<string>();
    [SerializeField] private bool ignoreCase = true;
    [SerializeField] private bool clearInputAfterSubmit = true;

    [Header("Events")]
    [SerializeField] private UnityEvent<string> onValidWord;
    [SerializeField] private UnityEvent<string> onInvalidWord;

    private HashSet<string> validWordSet;

    private void Awake()
    {
        BuildWordSet();
    }

    private void OnEnable()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(SubmitWord);
        }
    }

    private void OnDisable()
    {
        if (submitButton != null)
        {
            submitButton.onClick.RemoveListener(SubmitWord);
        }
    }

    private void BuildWordSet()
    {
        validWordSet = new HashSet<string>(ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

        for (int i = 0; i < validWords.Count; i++)
        {
            string word = Normalize(validWords[i]);
            if (!string.IsNullOrEmpty(word))
            {
                validWordSet.Add(word);
            }
        }
    }

    public void RefreshValidWords()
    {
        BuildWordSet();
    }

    public void SubmitWord()
    {
        if (inputField == null)
        {
            SetFeedback("Input field missing.");
            return;
        }

        string rawInput = inputField.text;
        string input = Normalize(rawInput);

        if (string.IsNullOrEmpty(input))
        {
            SetFeedback("Type a word.");
            onInvalidWord?.Invoke(rawInput);
            return;
        }

        if (IsValidWord(input))
        {
            SetFeedback(string.Empty);
            onValidWord?.Invoke(input);
        }
        else
        {
            SetFeedback("Invalid word.");
            onInvalidWord?.Invoke(input);
        }

        if (clearInputAfterSubmit)
        {
            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }
    }

    public bool IsValidWord(string word)
    {
        if (validWordSet == null)
        {
            BuildWordSet();
        }

        return validWordSet.Contains(Normalize(word));
    }

    public string GetMatchingWord(string word)
    {
        string normalized = Normalize(word);

        for (int i = 0; i < validWords.Count; i++)
        {
            string candidate = Normalize(validWords[i]);
            if (string.Equals(candidate, normalized, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return validWords[i];
            }
        }

        return string.Empty;
    }

    private string Normalize(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    private void SetFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
        }
    }
}
