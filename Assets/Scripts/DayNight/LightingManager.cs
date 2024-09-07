using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    // Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    // Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private float CycleDurationInMins = 5f; // Duration of one cycle in minutes

    private float cycleDurationInSeconds;
    private float startTime;

    private void Awake()
    {
        // Convert cycle duration from minutes to seconds
        cycleDurationInSeconds = CycleDurationInMins * 60f;
    }

    private void Start()
    {
        // Record the start time
        startTime = Time.time;
    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            // Calculate elapsed time since start
            float elapsedTime = Time.time - startTime;
            // Compute the current time of day based on elapsed time and cycle duration
            TimeOfDay = (elapsedTime / cycleDurationInSeconds) * 24f;
            TimeOfDay %= 24; // Ensure TimeOfDay wraps around 24 hours
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        // Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        // If the directional light is set then rotate and set its color
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    // Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        // Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        // Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
