using UnityEngine;
using TMPro;
using UnityEngine.Events;
using TopDown.Enemy;

namespace TopDown.Shooting
{
public class WordInputPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    public UnityEvent<string> onValidWord;
    
    private bool listenerRegistered = false;

    private void Awake()
    {
        Debug.Log("=== WordInputPanel.Awake() called! ===");
        // Register listener in Awake so it runs even if object is inactive
        if (!listenerRegistered)
        {
            Spawner.onWaveComplete.AddListener(OnWaveComplete);
            listenerRegistered = true;
            Debug.Log("✓ WordInputPanel listening to wave completion events");
        }
    }

    private void Start()
    {
        if (input != null)
        {//listen for submit events
            input.onSubmit.AddListener(HandleSubmit);
        }
        
        gameObject.SetActive(false); // Start inactive
    }

    private void OnDestroy()
    {
        // Clean up event listener
        if (listenerRegistered)
        {
            Spawner.onWaveComplete.RemoveListener(OnWaveComplete);
        }
    }

    private void OnWaveComplete()
    {
        // Wave is complete - activate this panel
        Debug.Log($"OnWaveComplete called! Panel active state before: {gameObject.activeSelf}");
        gameObject.SetActive(true);
        Debug.Log($"Panel active state after: {gameObject.activeSelf}");
    }

    private void OnEnable()
    {
        // When panel becomes active, focus the input field immediately
        if (input != null)
        {
            input.ActivateInputField();
            input.text = ""; // Clear any previous text
            Debug.Log("Word input panel enabled - ready for input!");
        }
    }

    private void HandleSubmit(string text)
    {//ignore empty or whitespace-only input
        if (string.IsNullOrWhiteSpace(text)) return;

        text = text.Trim().ToLower();//normalize input

        Debug.Log("Word entered: " + text);

        onValidWord?.Invoke(text);

        input.text = ""; //clear input
        input.ActivateInputField();//keep typing smoothly
    }
}
}