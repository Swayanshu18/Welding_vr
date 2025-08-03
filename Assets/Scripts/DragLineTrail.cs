using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(LineRenderer))]
public class DragLineTrail : MonoBehaviour
{
    public float minDistance = 0.01f;
    public Color lineColor = Color.red;
    public float lineWidth = 0.01f;

    public InputActionReference leftTriggerAction;
    public float triggerThreshold = 0.1f;

    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private Vector3 lastPosition;

    private bool isColliding = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lastPosition = transform.position;

        if (leftTriggerAction != null)
        {
            leftTriggerAction.action.Enable();
        }
    }

    void Update()
    {
        if (!isColliding || leftTriggerAction == null)
            return;

        float triggerValue = leftTriggerAction.action.ReadValue<float>();
        if (triggerValue > triggerThreshold)
        {
            float distance = Vector3.Distance(transform.position, lastPosition);
            if (distance >= minDistance)
            {
                AddPoint(transform.position);
                lastPosition = transform.position;
            }
        }
    }

    void AddPoint(Vector3 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
        lastPosition = transform.position; // Reset position for new trail
        points.Clear();
        lineRenderer.positionCount = 0;
        Debug.Log("Collision started: Line trail activated.");
    }

    void OnCollisionExit(Collision collision)
    {
        isColliding = false;
        Debug.Log("Collision ended: Line trail stopped.");
    }
}
