using UnityEngine;

/// <summary>
/// Keeps a GameObject facing toward the user's camera, similar to MRTK's Billboard but without dependencies.
/// </summary>
public class Billboard : MonoBehaviour
{
    public enum PivotAxis
    {
        Free,   // Rotates freely to face the user
        XY,     // Locks Z-axis
        XZ,     // Locks Y-axis
        YZ,     // Locks X-axis
    }

    [Tooltip("Specifies the axis about which the object will rotate.")]
    public PivotAxis pivotAxis = PivotAxis.Free;

    [Tooltip("Specifies the target we will orient to. If no target is specified, the main camera will be used.")]
    public Transform targetTransform;

    private void Start()
    {
        if (targetTransform == null && Camera.main != null)
        {
            targetTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (targetTransform == null) return;

        // Calculate direction from object to target
        Vector3 directionToTarget = targetTransform.position - transform.position;

        // Avoid zero-magnitude vectors
        if (directionToTarget.sqrMagnitude < 0.0001f) return;

        switch (pivotAxis)
        {
            case PivotAxis.XY:
                directionToTarget.z = 0; // Lock Z-axis
                break;
            case PivotAxis.XZ:
                directionToTarget.y = 0; // Lock Y-axis
                break;
            case PivotAxis.YZ:
                directionToTarget.x = 0; // Lock X-axis
                break;
            case PivotAxis.Free:
            default:
                break; // Allow full rotation
        }

        // Ensure valid rotation
        if (directionToTarget.sqrMagnitude < 0.0001f) return;

        // Apply rotation to face the camera
        transform.rotation = Quaternion.LookRotation(-directionToTarget, Vector3.up);
    }
}
