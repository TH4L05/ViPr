
/// <author>Thomas Krahl</author>

using System;

namespace TK.Util
{
    public class FPS
    {
        private float dt;

        public float GetFps(float UnityDeltaTime)
        {
            dt += 0.1f * (UnityDeltaTime - dt);
            float frames = 1.0f / dt;
            return Math.Clamp(frames, 0.0f, 999f);
        }
    }
}

