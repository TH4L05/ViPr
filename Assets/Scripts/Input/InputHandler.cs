/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using Valve.VR;

namespace eecon_lab.Input
{
    public class InputHandler : MonoBehaviour
    {
        #region SerializedField

        [SerializeField] bool enableMovmentInput;

        #endregion

        #region PrivateFields

        private GameControls gameControls;
        private List<InputAction> inputActionsMovement;

        #endregion

        #region PublicFields

        public static InputHandler Instance { get; private set; }
        public Vector2 MovementAxisInputValue => gameControls.Movement.BaseMovement.ReadValue<Vector2>();
        public Vector2 MouseAxisInputValue => gameControls.Movement.Rotation.ReadValue<Vector2>();
        public bool JumpInputValue => gameControls.Movement.Jump.WasPressedThisFrame();
        public bool SprintInputValue => gameControls.Movement.Sprint.WasPressedThisFrame();
        public bool CrouchInputValue => gameControls.Movement.Crouch.WasPressedThisFrame();
        public GameControls GameControls => gameControls;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            Intitialize();
        }

        private void Start()
        {
            SetInputActions();
            if(enableMovmentInput) EnableDisableInputActions(true, inputActionsMovement);
        }

        private void OnDestroy()
        {
            EnableDisableInputActions(false, inputActionsMovement);
        }

        #endregion

        #region Setup

        private void Intitialize()
        {
            gameControls  = new GameControls();
            inputActionsMovement = new List<InputAction>();
        }

        private void SetInputActions()
        {
            var inputMovement = gameControls.Movement;

            inputActionsMovement.Add(inputMovement.BaseMovement);
            inputActionsMovement.Add(inputMovement.Rotation);
            inputActionsMovement.Add(inputMovement.Jump);
            inputActionsMovement.Add(inputMovement.Sprint);
            inputActionsMovement.Add(inputMovement.Crouch);
        }

        public void EnableDisableInputActions(bool enable, List<InputAction> inputActionList)
        {
            if (enable)
            {
                foreach (InputAction inputAction in inputActionList)
                {
                    inputAction.Enable();
                }
            }
            else
            {
                foreach (InputAction inputAction in inputActionList)
                {
                    inputAction.Disable();
                }
            }
        }

        #endregion
    }
}
