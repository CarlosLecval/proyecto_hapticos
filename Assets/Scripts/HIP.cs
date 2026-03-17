using UnityEngine;

public class HIP : MonoBehaviour
{
    // establish Haptic Manager and IHIP objects
    public GameObject HMObject;

    // get haptic device information from the haptic manager
    private HM hapticManager;

    // haptic device variables
    public int hapticDeviceNumber;
    public int HapticDeviceNumber => hapticDeviceNumber;
    public Vector3 TargetPosition { get; private set; }

    void Start()
    {
        hapticManager = HMObject.GetComponent<HM>();
        TargetPosition = transform.position;
    }

    void FixedUpdate()
    {
        TargetPosition = hapticManager.GetPosition(hapticDeviceNumber);
        transform.position = TargetPosition;
    }

    // public GameObject IHIP;

    // private bool isColliding = false;
    // private Vector3 HIPCollidingPosition;

    /* void FixedUpdate()
    {
        // update position
        // position = hapticManager.GetPosition(hapticDeviceNumber);

        /* if (isColliding)
            IHIP.transform.position = HIPCollidingPosition;
        else
            IHIP.transform.position = position;
        // rigidBody.MovePosition(position);
        /*if (isColliding)
            rigidBody.MovePosition(HIPCollidingPosition);
        else
            rigidBody.MovePosition(position);

        transform.position = position;

        /* float maxLinearDamping = myHapticManager.GetHapticDeviceInfo(hapticDevice, 6);
        float maxAngularDamping = myHapticManager.GetHapticDeviceInfo(hapticDevice, 7);
        float maxGripperAngularDamping = myHapticManager.GetHapticDeviceInfo(hapticDevice, 8); 

        // update damping factors
        /* Kv = (Kv > maxLinearDamping) ? maxLinearDamping : Kv;
        Kvr = (Kvr > maxAngularDamping) ? maxAngularDamping : Kvr;
        Kvg = (Kvg > maxGripperAngularDamping) ? maxGripperAngularDamping : Kvg; 
    } */

    /* void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Object started collision with " + collision.gameObject.name);
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
        hapticManager.UpdateHipState(hapticDeviceNumber, false, position, Kp, Kv);
        Debug.Log("Object left collision with " + collision.gameObject.name);
    } 

    private void UpdateCollisionPositions(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);

        // The stable target is the HIP center resting on the contact surface.
        Vector3 surfacePosition = contact.point + (radius * contact.normal);

        HIPCollidingPosition = surfacePosition;
        hapticManager.UpdateHipState(hapticDeviceNumber, true, HIPCollidingPosition, Kp, Kv);
    } */
}
