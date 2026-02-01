/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.SpatialTracking;
using eecon_lab.Character.Movement;
using eecon_lab.XR;

namespace eecon_lab.Character.Player
{
    public class Player : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private Camera activeCamera;
        [SerializeField] private PlayerMovement playerMovement;

        [Header("VR")]
        [SerializeField] private Transform hmdTransform;
        [SerializeField] private TrackedPoseDriver.TrackingType trackingType = TrackedPoseDriver.TrackingType.RotationOnly;

        #endregion

        #region PrivateFields

        private Transform trackingOriginTransform;
        private CharacterController characterController;
        private bool useVR;
        private float eyeHeight;

        #endregion

        #region PublicFields

        public Camera ActiveCamera => activeCamera;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            SetupUnityXR.OnInitFinished += Setup;
        }

        public void Start()
        {                     
        }

        private void OnDestroy()
        {
            SetupUnityXR.OnInitFinished -= Setup;
        }

        #endregion

        public void Setup(bool vrActive)
        {
            characterController = GetComponent<CharacterController>();
            useVR = vrActive;

            if (useVR)
            {
                Debug.Log("<color=#A17FFF>USE VR</color>");
                TrackedPoseDriver trackedPoseDriver = activeCamera.transform.parent.gameObject.AddComponent<TrackedPoseDriver>();
                trackedPoseDriver.trackingType = trackingType;
                trackingOriginTransform = transform;
                playerMovement.isEnabled = false;
                characterController.enabled = false;
            }
            else
            {
                Debug.Log("<color=#A17FFF>USE Mouse and Keyboard</color>");
                if (!playerMovement.isEnabled) return;
                playerMovement.isEnabled = true;
            }
        }

        /*private void LateUpdate()
        {
            //if (useVR) GetEyeHeight();     
        }*/

        /*private void GetEyeHeight()
        {
            Transform hmd = hmdTransform;
            if (hmd)
            {
                Vector3 eyeOffset = Vector3.Project(hmd.position - trackingOriginTransform.position, trackingOriginTransform.up);
                eyeHeight = eyeOffset.magnitude / trackingOriginTransform.lossyScale.x;
                return;
            }
            eyeHeight = 0.0f;
        }*/

        /*private void OnDrawGizmosSelected()
        {
            Vector3 position = new Vector3(hmdTransform.position.x, eyeHeight, hmdTransform.position.z);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, 0.20f);
        }*/

    }
}

