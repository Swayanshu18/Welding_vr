using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class WeldingTorch : MonoBehaviour
{
    [Header("Trigger & Timing Settings")]
    public InputActionReference leftTriggerAction;
    public float triggerThreshold = 0.1f;
    public float delayBetweenObjects = 0.5f;

    [Header("Object Reveal Mapping")]
    public List<CollisionRevealMapping> collisionReveals;  // define mapping in inspector

    private bool isColliding = false;
    private bool coroutineStarted = false;
    private bool hasAlreadyShown = false;

    private string currentCollidedObjectName = "";
    private Dictionary<string, List<GameObject>> revealMap;

    void Start()
    {
        BuildRevealMap();
        HideAllObjects();
    }

    void OnEnable()
    {
        leftTriggerAction?.action.Enable();
    }

    void OnDisable()
    {
        leftTriggerAction?.action.Disable();
    }

    void Update()
    {
        if (!hasAlreadyShown && isColliding && !coroutineStarted && leftTriggerAction != null)
        {
            float triggerValue = leftTriggerAction.action.ReadValue<float>();

            if (triggerValue > triggerThreshold && revealMap.ContainsKey(currentCollidedObjectName))
            {
                coroutineStarted = true;
                hasAlreadyShown = true;
                StartCoroutine(ShowObjectsOneByOne(revealMap[currentCollidedObjectName]));
                Debug.Log($"Trigger pressed while colliding with: {currentCollidedObjectName}");
            }
        }
    }

    IEnumerator ShowObjectsOneByOne(List<GameObject> objectsToShow)
    {
        foreach (GameObject obj in objectsToShow)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log($"Object made visible: {obj.name}");
                yield return new WaitForSeconds(delayBetweenObjects);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
        currentCollidedObjectName = collision.gameObject.name;

        Debug.Log("Collided with: " + currentCollidedObjectName);
    }

    void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }

    void HideAllObjects()
    {
        foreach (var mapping in collisionReveals)
        {
            foreach (var obj in mapping.objectsToShow)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }
    }

    void BuildRevealMap()
    {
        revealMap = new Dictionary<string, List<GameObject>>();

        foreach (var mapping in collisionReveals)
        {
            if (!revealMap.ContainsKey(mapping.collidedObjectName))
            {
                revealMap[mapping.collidedObjectName] = new List<GameObject>();
            }

            revealMap[mapping.collidedObjectName].AddRange(mapping.objectsToShow);
        }
    }

    [System.Serializable]
    public class CollisionRevealMapping
    {
        public string collidedObjectName;           // e.g., "Object1", "Object2"
        public List<GameObject> objectsToShow;      // which objects to show when collided with this
    }
}
