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

    // IHIP variables
    private Rigidbody rigidBody;
    private float radius;

    private Vector3 totalForce = Vector3.zero;
    private bool isCollidingWithBound = false;

    void Start()
    {
        hapticManager = HMObject.GetComponent<HM>();
        rigidBody = GetComponent<Rigidbody>();
        radius = GetComponent<Renderer>().bounds.extents.magnitude / 2;

        HIPObject.transform.position = hapticManager.GetPosition(numHapDev);
        rigidBody.position = HIPObject.transform.position;
    }

    void FixedUpdate()
    {
        Vector3 position = hapticManager.GetPosition(numHapDev);
        HIPObject.transform.position = position;

        Vector3 newPosition = position;

        if (position.y < radius - .5f)
            newPosition.y = radius - .5f;
        else if (position.y > 29.5f - radius)
            newPosition.y = 29.5f - radius;

        if (position.x < -35f + radius)
            newPosition.x = -35f + radius;
        else if (position.x > 35f - radius)
            newPosition.x = 35f - radius;

        if (position.z < -25f + radius)
            newPosition.z = -25f + radius;
        else if (position.z > 25f - radius)
            newPosition.z = 25f - radius;

        if (rigidBody.position != newPosition)
            rigidBody.MovePosition(newPosition);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ball")
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-collision.GetContact(0).normal * 10f, ForceMode.Impulse);

        UpdateCollisionForce(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        UpdateCollisionForce(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        isCollidingWithBound = false;
        totalForce = Vector3.zero;
        hapticManager.UpdateCollisionState(numHapDev, Vector3.zero);
    }

    private void UpdateCollisionForce(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);

        if (collision.gameObject.name == "Grass" || collision.gameObject.name.Contains("Bound"))
        {
            isCollidingWithBound = true;
            totalForce = contact.normal * 2.5f;
        }
        else if (collision.gameObject.name == "Ball")
        {
            if (isCollidingWithBound)
                totalForce += contact.normal;
            else
                totalForce = contact.normal * 1.5f;
        }

        hapticManager.UpdateCollisionState(numHapDev, totalForce);
    }
}
