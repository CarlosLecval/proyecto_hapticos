using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class IHIP : MonoBehaviour
{
    public GameObject HMObject;
    public GameObject HIPObject;

    private HM hapticManager;

    // Haptic device variables
    [Header("Haptic Device Number")]
    public int numHapDev;
    private Vector3 actualPosition;

    // IHIP variables
    private Vector3 collisionPosition;
    private bool isColliding = false;
    private Rigidbody rigidBody;
    private float radius;

    // Physics coefficients
    public float springConstant = 50; // [N/m] max: 1000
    public float dampingCoefficient = 10; // [N/m] max: 20

    void Start()
    {
        hapticManager = HMObject.GetComponent<HM>();
        rigidBody = GetComponent<Rigidbody>();
        radius = GetComponent<Renderer>().bounds.extents.magnitude / 2;
        actualPosition = HIPObject.transform.position;
        /* SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
            radius = sphereCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        else */
    }

    void FixedUpdate()
    {
        actualPosition = hapticManager.GetPosition(numHapDev);
        HIPObject.transform.position = actualPosition;

        if (rigidBody.position != actualPosition)
        {
            rigidBody.MovePosition(actualPosition);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
        UpdateCollisionPositions(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        UpdateCollisionPositions(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        isColliding = false;
        collisionPosition = rigidBody.position;
        hapticManager.UpdateCollisionState(numHapDev, false, collisionPosition, springConstant, dampingCoefficient);
    }

    private void UpdateCollisionPositions(Collision collision)
    {
        if (collision.contactCount == 0) return;

        ContactPoint contact = collision.GetContact(0);
        collisionPosition = contact.point + (radius * contact.normal);
        hapticManager.UpdateCollisionState(numHapDev, true, collisionPosition, springConstant, dampingCoefficient);
    }
}
