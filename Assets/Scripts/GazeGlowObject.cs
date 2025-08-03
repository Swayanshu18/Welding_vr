using UnityEngine;

public class GazeGlowObject : MonoBehaviour
{
    public Color glowColor = Color.cyan;  // Glow color
    public float emissionIntensity = 1.5f;

    private Material originalMaterial;
    private Color originalEmissionColor;
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalMaterial = objectRenderer.material;

        if (originalMaterial.HasProperty("_EmissionColor"))
        {
            originalEmissionColor = originalMaterial.GetColor("_EmissionColor");
        }

        // Ensure emission is active
        originalMaterial.EnableKeyword("_EMISSION");
    }

    public void OnGazeEnter()
    {
        objectRenderer.material.SetColor("_EmissionColor", glowColor * emissionIntensity);
    }

    public void OnGazeExit()
    {
        objectRenderer.material.SetColor("_EmissionColor", originalEmissionColor);
    }
}
