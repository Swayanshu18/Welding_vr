using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using System.Collections.Generic;

public class WeldingController : MonoBehaviour
{
    public List<ParticleSystem> particleEffects;  // List of particle systems
    public XRGrabInteractable grabInteractable;
    public InputActionProperty leftTriggerAction;  // Use InputActionProperty for left trigger
    public AudioSource weldingSound;               // Drag AudioSource with welding sound here

    private bool isGrabbed = false;
    private bool wasTriggerPressed = false;

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        leftTriggerAction.action.Enable();  // Make sure action is enabled
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
        leftTriggerAction.action.Disable();
    }

    void Update()
    {
        float triggerValue = leftTriggerAction.action.ReadValue<float>();
        bool isTriggerPressed = triggerValue > 0.1f;

        if (isGrabbed && isTriggerPressed)
        {
            foreach (var effect in particleEffects)
            {
                if (!effect.isPlaying)
                    effect.Play();
            }

            if (!weldingSound.isPlaying && !wasTriggerPressed)
                weldingSound.Play();
        }
        else
        {
            foreach (var effect in particleEffects)
            {
                if (effect.isPlaying)
                    effect.Stop();
            }

            if (weldingSound.isPlaying)
                weldingSound.Stop();
        }

        wasTriggerPressed = isTriggerPressed;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        foreach (var effect in particleEffects)
        {
            if (effect.isPlaying)
                effect.Stop();
        }

        if (weldingSound.isPlaying)
            weldingSound.Stop();
    }
}
