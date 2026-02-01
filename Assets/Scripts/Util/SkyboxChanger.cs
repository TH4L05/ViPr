/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


namespace eecon_lab.Rendering
{
    public class SkyboxChanger : MonoBehaviour
    {
        #region SkyboxSetup

        [System.Serializable]
        public struct SkyboxSetup
        {
            [Header("Material")]
            public Material skyboxMaterial;
            public bool skyLightEnabled;

            [Header("Light")]
            public Vector3 skyLightRotation;
            public Color lightColor;
            public float colorTemperature;
            public float skyLightIntensity;
            public LightShadows lightShadows;
            [Range(0.0f, 1.0f)] public float shadowIntensity;

            [Header("Fog")]
            public bool fogEnabled;
            public Color fogColor;
            public float fogDensity;
            //public FogMode fogMode;

            [Header("Volume")]
            public VolumeProfile volumeProfile;

            public void SetSkybox()
            {
                RenderSettings.skybox = skyboxMaterial;
            }

            public void SetSkyLightState(GameObject directionalLight)
            {
                if (directionalLight == null) return;
                directionalLight.SetActive(skyLightEnabled);
            }

            public void SetSkyLightRotation(GameObject directionalLight)
            {
                if (directionalLight == null) return;
                directionalLight.transform.eulerAngles = skyLightRotation;
            }

            public void SetLight(Light directionalLight)
            {
                if (directionalLight == null) return;
                directionalLight.intensity = skyLightIntensity;
                directionalLight.color = lightColor;
                directionalLight.colorTemperature = colorTemperature;
                directionalLight.shadows = lightShadows;
                directionalLight.shadowStrength = shadowIntensity;
            }

            public void SetVolume(Volume volume)
            {
                volume.profile = volumeProfile;
            }

            public void SetFog()
            {
                RenderSettings.fog = fogEnabled;
                RenderSettings.fogColor = fogColor;
                //RenderSettings.fogMode = fogMode;
                RenderSettings.fogDensity = fogDensity;
            }
        }

        #endregion

        #region SerializedFields

        [SerializeField] private GameObject directionalLightObject;
        [SerializeField] private Volume globalVolume;
        [SerializeField] private List<SkyboxSetup> skyboxSetups = new List<SkyboxSetup>();
        [SerializeField] private List<ReflectionProbe> sceneReflectionprobes = new List<ReflectionProbe>();

        #endregion

        #region PrivateFields

        private CustomEnvironmentLigthing customEnvironmentLigthing;

        #endregion

        private void Awake()
        {
            customEnvironmentLigthing = GetComponentInChildren<CustomEnvironmentLigthing>();
            customEnvironmentLigthing.Set();
        }

        public void ChangeSkybox(int listIndex)
        {
            if (listIndex < 0 || listIndex > skyboxSetups.Count) return;

            if (directionalLightObject != null)
            {
                skyboxSetups[listIndex].SetSkyLightState(directionalLightObject);

                if (skyboxSetups[listIndex].skyLightEnabled)
                {
                    skyboxSetups[listIndex].SetSkyLightRotation(directionalLightObject);
                    Light skyLight = directionalLightObject.GetComponent<Light>();
                    skyboxSetups[listIndex].SetLight(skyLight);
                }
            }
            
            skyboxSetups[listIndex].SetVolume(globalVolume);
            skyboxSetups[listIndex].SetSkybox();
            skyboxSetups[listIndex].SetFog();

            customEnvironmentLigthing.Set();

            DynamicGI.UpdateEnvironment();
            UpdateReflectionProbes();
        }

        private void UpdateReflectionProbes()
        {
            foreach (ReflectionProbe probe in sceneReflectionprobes)
            {
                probe.RenderProbe();
            }
        }
    }
}

