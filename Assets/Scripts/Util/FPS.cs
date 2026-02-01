/// <author>Thomas Krahl</author>

using UnityEngine;

namespace TK.Util
{
    public class FPS
    {
        private float dt;

        private float GetFps()
        {
            dt += 0.1f * (Time.deltaTime - dt);
            float frames = 1.0f / dt;
            return Mathf.Clamp(frames, 0.0f, 999f);
        }
    }
}

