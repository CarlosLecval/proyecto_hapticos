using System;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public static event Action<int> OnGoalScored;
    [SerializeField] private int teamNumber;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collider.gameObject.GetComponent<Rigidbody>();
            if (ballRb == null)
            {
                return;
            }

            ballRb.MovePosition(Vector3.zero);
            ballRb.linearVelocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            OnGoalScored?.Invoke(teamNumber);
        }
    }
}