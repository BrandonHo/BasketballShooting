using UnityEngine;

/// <summary>
/// Component which serves as a controller for ball game objects. This component
/// is primarily used to apply force to a ball game object, or to apply changes
/// to the position and velocity of the ball game object.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    private Rigidbody RigidbodyComponent;       // Physics component of the ball, used to apply forces to the ball.
    private Vector3 InitialPosition;            // The initial position of the ball, used for resetting the ball position.
    private Quaternion InitialRotation;         // The initial rotation of the ball, used for resetting the ball rotation.
    private Transform TargetTransform;          // Transform component of the target game object.
    private float MinDistanceToTarget;          // The minimum distance between the ball and the target game object for the current generation.
    public bool IsScored = false;               // Boolean which indicates if the ball has successfully collided with a basketball hoop

    void Awake()
    {
        // Initialisation - reference rigidbody component, and initialise ball parameters
        RigidbodyComponent = GetComponent<Rigidbody>();
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
        MinDistanceToTarget = float.MaxValue;
    }

    void FixedUpdate()
    {
        // Update minimum distance from ball to the target (basketball hoop)
        if (TargetTransform)
        {
            float distanceToTarget = Vector3.Distance(TargetTransform.position, transform.position);
            if (distanceToTarget < MinDistanceToTarget)
                MinDistanceToTarget = distanceToTarget;
        }
    }

    /// <summary>
    /// Utility method for applying forces to the ball game object.
    /// </summary>
    /// <param name="upwardForce">Upward force to be applied to the ball game object.</param>
    /// <param name="forwardForce">Forward force to be applied to the ball game object.</param>
    public void ApplyForce(float upwardForce, float forwardForce)
    {
        RigidbodyComponent.AddRelativeForce(new Vector3(0f, upwardForce, forwardForce), ForceMode.Impulse);
    }

    /// <summary>
    /// Utility method for resetting the ball in prepration for evaluating a new generation of chromosomes.
    /// </summary>
    public void Reset()
    {
        IsScored = false;
        MinDistanceToTarget = float.MaxValue;
        ResetVelocity();
        SetPosition(InitialPosition);
    }

    /// <summary>
    /// Utility method for resetting the velocity of the ball.
    /// </summary>
    public void ResetVelocity()
    {
        RigidbodyComponent.velocity = Vector3.zero;
        RigidbodyComponent.angularVelocity = Vector3.zero;
    }

    /// <summary>
    /// Utility method for setting the position of the ball.
    /// </summary>
    /// <param name="targetPosition">Position to be set for the ball.</param>
    public void SetPosition(Vector3 targetPosition)
    {
        transform.SetPositionAndRotation(targetPosition, InitialRotation);
    }

    /// <summary>
    /// Setter method for updating the target transform, which is used for calculating minimum distances.
    /// </summary>
    /// <param name="newTarget">Target transform to be set.</param>
    public void SetTarget(Transform newTarget)
    {
        TargetTransform = newTarget;
    }

    /// <summary>
    /// Getter method for retrieving the minimum distance to target.
    /// </summary>
    /// <returns>The minimum distance to the target.</returns>
    public float GetMinDistanceToTarget()
    {
        return MinDistanceToTarget;
    }

}
