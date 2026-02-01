/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace eecon_lab.Input.Extras
{
    public class CustomFunctionKeyEvents : MonoBehaviour
    {
        [SerializeField] private bool isEnabled = true;

        public UnityEvent OnF1pressed;
        public UnityEvent OnF2pressed;
        public UnityEvent OnF3pressed;
        public UnityEvent OnF4pressed;
        public UnityEvent OnF5pressed;
        public UnityEvent OnF6pressed;
        public UnityEvent OnF7pressed;
        public UnityEvent OnF8pressed;
        public UnityEvent OnF9pressed;
        public UnityEvent OnF10pressed;
        public UnityEvent OnF11pressed;
        public UnityEvent OnF12pressed;


        private void LateUpdate()
        {
            if (!isEnabled) return;
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                OnF1pressed?.Invoke();
            }
            if (Keyboard.current.f2Key.wasPressedThisFrame)
            {
                OnF2pressed?.Invoke();
            }
            if (Keyboard.current.f3Key.wasPressedThisFrame)
            {
                OnF3pressed?.Invoke();
            }
            if (Keyboard.current.f4Key.wasPressedThisFrame)
            {
                OnF4pressed?.Invoke();
            }
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                OnF5pressed?.Invoke();
            }
            if (Keyboard.current.f6Key.wasPressedThisFrame)
            {
                OnF6pressed?.Invoke();
            }
            if (Keyboard.current.f7Key.wasPressedThisFrame)
            {
                OnF7pressed?.Invoke();
            }
            if (Keyboard.current.f8Key.wasPressedThisFrame)
            {
                OnF8pressed?.Invoke();
            }
            if (Keyboard.current.f9Key.wasPressedThisFrame)
            {
                OnF9pressed?.Invoke();
            }
            if (Keyboard.current.f10Key.wasPressedThisFrame)
            {
                OnF10pressed?.Invoke();
            }
            if (Keyboard.current.f11Key.wasPressedThisFrame)
            {
                OnF11pressed?.Invoke();
            }
            if (Keyboard.current.f12Key.wasPressedThisFrame)
            {
                OnF12pressed?.Invoke();
            }
        }
    }
}

