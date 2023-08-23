using UnityEngine;

public class PulseUIImageAndLight : MonoBehaviour
{
    public float pulseSpeed = 1.0f;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    public Light pulseLight; // Reference to the Light component

    private Vector3 initialScale;
    private float initialLightIntensity;

    private void Start()
    {
        initialScale = transform.localScale;

        if (pulseLight != null)
        {
            initialLightIntensity = pulseLight.intensity;
        }
    }

    private void Update()
    {
        // Calculate the new scale based on a sine wave
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * pulseSpeed, 1.0f));

        // Apply the new scale to the image
        transform.localScale = initialScale * scale;

        if (pulseLight != null)
        {
            // Calculate the new light intensity based on a sine wave
            float lightIntensity = Mathf.Lerp(initialLightIntensity * minScale, initialLightIntensity * maxScale, Mathf.PingPong(Time.time * pulseSpeed, 1.0f));

            // Apply the new light intensity
            pulseLight.intensity = lightIntensity;
        }
    }
}
