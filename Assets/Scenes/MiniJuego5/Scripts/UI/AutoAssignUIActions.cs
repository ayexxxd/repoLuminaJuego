using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

namespace TopDown.UI
{
    public class AutoAssignUIActions : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActions;

        void Start()
        {
            if (inputActions == null)
            {
                // Try to find the default GameControls asset
                var allAssets = Resources.FindObjectsOfTypeAll<InputActionAsset>();
                foreach (var asset in allAssets)
                {
                    if (asset.name.Contains("GameControls") || asset.name.Contains("UI"))
                    {
                        inputActions = asset;
                        break;
                    }
                }
            }

            if (inputActions == null)
            {
                Debug.LogWarning("AutoAssignUIActions: No InputActionAsset found.");
                return;
            }

            EventSystem es = EventSystem.current;
            if (es == null)
            {
                Debug.LogWarning("AutoAssignUIActions: No EventSystem found.");
                return;
            }

            InputSystemUIInputModule module = es.GetComponent<InputSystemUIInputModule>();
            if (module == null)
            {
                Debug.LogWarning("AutoAssignUIActions: No InputSystemUIInputModule found on EventSystem.");
                return;
            }

            // Find the UI action map
            InputActionMap uiMap = inputActions.FindActionMap("UI");
            if (uiMap == null)
            {
                Debug.LogWarning("AutoAssignUIActions: No 'UI' action map found in asset.");
                return;
            }

            // Enable the UI map
            uiMap.Enable();

            // Assign actions by name
            AssignAction(module, uiMap, "Point",        (m, a) => m.point = a);
            AssignAction(module, uiMap, "LeftClick",    (m, a) => m.leftClick = a);
            AssignAction(module, uiMap, "Submit",       (m, a) => m.submit = a);
            AssignAction(module, uiMap, "Cancel",       (m, a) => m.cancel = a);

            Debug.Log("AutoAssignUIActions: UI actions assigned successfully.");
        }

        private void AssignAction(InputSystemUIInputModule module, InputActionMap map, string actionName, System.Action<InputSystemUIInputModule, InputActionReference> setter)
        {
            InputAction action = map.FindAction(actionName);
            if (action != null)
            {
                InputActionReference reference = InputActionReference.Create(action);
                setter(module, reference);
            }
            else
            {
                Debug.LogWarning($"AutoAssignUIActions: Action '{actionName}' not found in UI map.");
            }
        }
    }
}
