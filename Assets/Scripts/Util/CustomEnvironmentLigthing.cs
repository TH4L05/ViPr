/// <author>Thomas Krahl</author>

using UnityEngine;

namespace eecon_lab.Rendering
{
    public class CustomEnvironmentLigthing : MonoBehaviour
    {
        public Cubemap cubemap;
        public ReflectionProbe probe;
        public Camera tempCamera;

        public void Set()
        {
            tempCamera.gameObject.SetActive(true);
            tempCamera.RenderToCubemap(cubemap);
            tempCamera.gameObject.SetActive(false);
        }
    }
}

