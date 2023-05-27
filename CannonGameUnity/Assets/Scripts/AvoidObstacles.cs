using UnityEngine;

public class AvoidObstacles : MonoBehaviour
{
    public float sphereCastRadius;
    public float avoidanceDistance;
    public LayerMask obstacleLayer;
    public Transform avoidanceDetectionOrigin;

    public Vector3 CheckForObstacles()
    {
        RaycastHit hit;
        if (Physics.SphereCast(avoidanceDetectionOrigin.position, sphereCastRadius, transform.forward, out hit, avoidanceDistance, obstacleLayer))
        {
            // Calculate a new direction perpendicular to the obstacle normal
            Vector3 avoidanceDirection = Vector3.Cross(transform.forward, hit.normal);
            return avoidanceDirection.normalized;
        }
        else
        {
            // No obstacles detected, continue moving in the original direction
            return Vector3.zero;
        }
    }
}
