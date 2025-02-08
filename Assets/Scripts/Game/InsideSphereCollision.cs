using UnityEngine;

public class InternalCollisionHandler : MonoBehaviour
{
    public GameObject bigSphere;
    private SphereCollider bigSphereCollider;
    private SphereCollider smallSphereCollider;

    void Start()
    {
        bigSphereCollider = bigSphere.GetComponent<SphereCollider>();
        smallSphereCollider = GetComponent<SphereCollider>();
    }

    void FixedUpdate()
    {
        HandleInternalCollision();
    }

    void HandleInternalCollision()
    {
        Vector3 directionToCenter = (bigSphere.transform.position - transform.position).normalized;
        float distanceToCenter = Vector3.Distance(bigSphere.transform.position, transform.position);
        float bigSphereRadius = bigSphereCollider.radius * bigSphere.transform.localScale.x / 2; // Adjust for scale
        float smallSphereRadius = smallSphereCollider.radius * transform.localScale.x / 2; // Adjust for scale

        if (distanceToCenter + smallSphereRadius > bigSphereRadius)
        {
            // Calculate the penetration depth
            float penetrationDepth = (distanceToCenter + smallSphereRadius) - bigSphereRadius;

            // Adjust the position to simulate collision response
            transform.position -= directionToCenter * penetrationDepth;

            // Optionally, apply some reaction force to simulate bounce (if needed)
            Rigidbody smallSphereRigidbody = GetComponent<Rigidbody>();
            smallSphereRigidbody.AddForce(-directionToCenter * penetrationDepth * 10f, ForceMode.Impulse);
        }
    }
}
