/// <author>Thomas Krahl</author>

using UnityEngine;

namespace eecon_lab.Character.Movement
{
    [CreateAssetMenu(fileName = "NewMovementData", menuName = "Data/Character/MovmentData")]
    public class MovementData : ScriptableObject
    {
        [Range(1f, 99f)] public float gravity = 9.81f;
        [Range(1f, 50f)] public float jump_force = 7f;
        [Range(1f, 20f)] public float move_speed = 5f;
        [Range(1f, 20f)] public float sprint_speed = 7.5f;
        [Range(1f, 20f)] public float crouch_speed = 2.5f;
        [Range(1f, 20f)] public float slide_speed = 2.5f;
        [Range(0.1f, 20f)] public float drag = 2.2f;
        [Range(0.1f, 20f)] public float groundfriction = 4.5f;
        [Range(0.01f, 1f)] public float aerial_control = 1f;
        [Range(0f, 2f)] public float jumpBufferTimeframe = 0.2f;
        [Range(0f, 2f)] public float coyoteTimeframe = 0.15f;

        [Range(-105.0f, 0f)] public float lookLimitUp = -85.0f;
        [Range(0f, 105.0f)] public float lookLimitDown = 85.0f;

        [Range(0.1f, 10f)] public float stepDistanceWalk = 1.0f;
        [Range(0.1f, 10f)] public float stepDistnaceRun = 0.5f;
        [Range(0.1f, 10f)] public float stepDistnaceCrouch = 1.5f;

        [Header("VR")]
        [Range(1f, 360f)] public float snapAngle = 30.0f;
        [Range(0.1f, 2f)] public float canTurnEverySeconds = 0.4f;
    }
}
