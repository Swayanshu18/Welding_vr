
# Welding VR Simulation – Project Explanation

## Overview

This Unity project simulates a welding scenario using VR interactions. The user can interact with virtual objects using a welding torch, triggering visual effects and revealing object parts conditionally. The project includes gaze-based object highlighting and two different spark particle systems.

---

## 1. WeldingTorch.cs – Dictionary-Based Reveal System

The core mechanic centers around revealing parts of an object only when the welding torch is in contact with specific objects and the left trigger is pressed.

- A dictionary (`Dictionary<string, List<GameObject>> revealMap`) is constructed at runtime to map object names (that the torch collides with) to the list of GameObjects that should be revealed.
- This setup is managed using a serializable class `CollisionRevealMapping`, allowing assignments to be done through the Unity Inspector.
- When the torch collides with a predefined object and the trigger is pressed beyond a threshold, a coroutine is triggered to reveal the associated GameObjects one by one with a delay.
- There's also an optional test setup: pressing the `1` key reveals objects tied to the name `"Object1"` manually.

This approach avoids hardcoded logic, making the scene behavior more modular, data-driven, and easier to extend.

---

## 2. Gaze Detection and Glow Feedback

To enhance user feedback, the project includes a gaze detection system that visually highlights objects the user is looking at.

### GazeDetector.cs

- Casts a ray from the user's viewpoint every frame.
- Detects whether the ray is hitting any object within a specified distance.
- If the gaze switches to a new object, the previous object stops glowing and the new one is highlighted.

### GazeGlowObject.cs

- Handles the visual glow effect using Unity's emission color.
- When the object is gazed at, it increases the emission intensity to produce a glow.
- Once the user looks away, the glow is reverted to the original material state.

This system is useful for intuitive navigation and feedback in immersive environments.

---

## 3. Particle Effects – Spark Systems

The welding effect includes two types of spark particle systems, allowing for flexibility in visuals or optimization:

### Custom Spark Effect

- Created manually using Unity’s built-in Particle System.
- Uses additive blending and simple parameters for lifetime, speed, size, and emission.
- Lightweight and ideal for fine control and optimization.

### Downloaded Spark Effect

- Sourced from an external package or the Unity Asset Store.
- Often includes more detailed visuals such as trail renderers, lighting, or sound.
- Suitable for higher-end setups or when aiming for more realism.

Both versions can be used interchangeably depending on the desired visual outcome or performance constraints.

---

## Project Behavior Summary

1. The user grabs the welding torch.
2. On colliding with a specific object, the script checks for matching entries in the dictionary.
3. If the left trigger is pressed while colliding, the relevant parts are revealed gradually using a coroutine.
4. Meanwhile, gaze detection continuously highlights objects being looked at with a glow effect.
5. Particle sparks can be triggered during welding for visual feedback (to be integrated where necessary).

---

## Scripts Included

| Script               | Functionality                                          |
|----------------------|--------------------------------------------------------|
| `WeldingTorch.cs`    | Handles collision detection, trigger input, and object reveal logic using a dictionary. |
| `GazeDetector.cs`    | Raycasts from the user's view to determine which object is being looked at. |
| `GazeGlowObject.cs`  | Applies and resets emission-based glow when gazed at.  |

## Demo Video

[![Watch the demo](https://img.youtube.com/vi/Upkll3EkJeY/0.jpg)](https://www.youtube.com/watch?v=Upkll3EkJeY)
