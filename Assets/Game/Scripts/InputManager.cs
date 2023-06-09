using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace TTProject.Manager
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput PlayerInput;

        public Vector2 Move { get; private set; }
        public Vector2 Look { get; private set; }
        public bool Run { get; private set; }

        private InputActionMap currentMap;
        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction runAction;

        private void Awake()
        {
            currentMap = PlayerInput.currentActionMap;
            moveAction = currentMap.FindAction("Move");
            lookAction = currentMap.FindAction("Look");
            runAction = currentMap.FindAction("Run");

            moveAction.performed += onMove;
            lookAction.performed += onLook;
            runAction.performed += onRun;

            moveAction.canceled += onMove;
            lookAction.canceled += onLook;
            runAction.canceled += onRun;
        }

        #region Callbacks
        private void onMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void onLook(InputAction.CallbackContext context)
        {
            Look = context.ReadValue<Vector2>();
        }

        private void onRun(InputAction.CallbackContext context)
        {
            Run = context.ReadValueAsButton();
        }
        #endregion

        private void OnEnable()
        {
            currentMap.Enable();
        }

        private void OnDisable()
        {
            currentMap.Disable();
        }
    }
}
