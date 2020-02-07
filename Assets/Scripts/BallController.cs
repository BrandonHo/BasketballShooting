using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody RigidbodyComp;
    private Vector3 InitialPosition;
    private Quaternion InitialRotation;
    public bool IsScored = false;

    private Transform Target;
    private float MinDistanceToTarget = float.MaxValue;

    void Awake()
    {
        RigidbodyComp = GetComponent<Rigidbody>();
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        if (Target)
        {
            float distanceToTarget = Vector3.Distance(Target.transform.position, transform.position);
            if (distanceToTarget < MinDistanceToTarget)
                MinDistanceToTarget = distanceToTarget;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        Target = newTarget;
    }

    public float GetMinDistanceToTarget()
    {
        return MinDistanceToTarget;
    }

    public void ApplyForce(float upwardForce, float forwardForce)
    {
        RigidbodyComp.AddRelativeForce(new Vector3(0f, upwardForce, forwardForce), ForceMode.Impulse);
    }

    public void Reset()
    {
        IsScored = false;
        MinDistanceToTarget = float.MaxValue;
        ResetToPosition(InitialPosition, InitialRotation);
    }

    public void ResetToPosition(Vector3 targetPosition)
    {
        ResetToPosition(targetPosition, InitialRotation);
    }

    public void ResetToPosition(Vector3 targetPosition, Quaternion resetRotation)
    {
        RigidbodyComp.velocity = Vector3.zero;
        RigidbodyComp.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(targetPosition, resetRotation);
    }
}
