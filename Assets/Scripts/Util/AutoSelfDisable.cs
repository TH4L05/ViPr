/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Events;

namespace TK.Util
{
    public class AutoSelfDisable : MonoBehaviour
    {
        [SerializeField] private bool isEnabled;
        [SerializeField] private float activeTime = 5.0f;
        [Space(10.0f)]public UnityEvent OnActiveTimeReached;

        private float currentActiveTime;
        private bool active;


        private void OnEnable()
        {
            if (!isEnabled) return;
            active = true;
            currentActiveTime = 0;
        }

        private void OnDisable()
        {
            active = false;
            currentActiveTime = 0;
        }

        private void LateUpdate()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (!isEnabled) return;
            if (!active) return;

            currentActiveTime += Time.deltaTime;

            if (currentActiveTime > activeTime)
            {
                OnActiveTimeReached?.Invoke();
                gameObject.SetActive(false);
            }

        }

    }
}

