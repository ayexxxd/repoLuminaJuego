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
                return;

            EventSystem es = EventSystem.current;
            if (es == null)
                return;

            InputSystemUIInputModule module = es.GetComponent<InputSystemUIInputModule>();
            if (module == null)
                return;

            InputActionMap uiMap = inputActions.FindActionMap("UI");
            if (uiMap == null)
                return;

            uiMap.Enable();

            AssignAction(module, uiMap, "Point",        (m, a) => m.point = a);
            AssignAction(module, uiMap, "LeftClick",    (m, a) => m.leftClick = a);
            AssignAction(module, uiMap, "Submit",       (m, a) => m.submit = a);
            AssignAction(module, uiMap, "Cancel",       (m, a) => m.cancel = a);
        }

        private void AssignAction(InputSystemUIInputModule module, InputActionMap map, string actionName, System.Action<InputSystemUIInputModule, InputActionReference> setter)
        {
            InputAction action = map.FindAction(actionName);
            if (action != null)
            {
                InputActionReference reference = InputActionReference.Create(action);
                setter(module, reference);
            }
        }
    }
}
