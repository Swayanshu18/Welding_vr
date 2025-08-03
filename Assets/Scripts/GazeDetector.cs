using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
public class GazeDetector : MonoBehaviour
{
    public float gazeDistance = 10f;
    private GameObject currentGazedObject;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * gazeDistance, Color.red);  // 👈 Show ray in Scene view

        if (Physics.Raycast(ray, out hit, gazeDistance))
        {
            Debug.Log("Gazing at: " + hit.collider.name);  // 👈 Log what object you are hitting
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentGazedObject)
            {
                if (currentGazedObject != null)
                {
                    var oldGlow = currentGazedObject.GetComponent<GazeGlowObject>();
                    if (oldGlow) oldGlow.OnGazeExit();
                }

                currentGazedObject = hitObject;
                var glow = currentGazedObject.GetComponent<GazeGlowObject>();
                if (glow) glow.OnGazeEnter();
            }
        }
        else
        {
            if (currentGazedObject != null)
            {
                var oldGlow = currentGazedObject.GetComponent<GazeGlowObject>();
                if (oldGlow) oldGlow.OnGazeExit();
                currentGazedObject = null;
            }
        }
    }

}

