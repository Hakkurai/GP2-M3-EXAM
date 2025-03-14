using UnityEngine;

public class CueBallCamera : MonoBehaviour
{
    public Transform cueBall; // Assign the cue ball in Inspector
    public float smoothSpeed = 5f; // Smooth transition speed
    public Vector3 offset = new Vector3(0, 3, -5); // Camera offset

    private Rigidbody cueBallRb;

    void Start()
    {
        if (cueBall != null)
            cueBallRb = cueBall.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (cueBall == null) return;

        // Follow the cue ball only if it's moving
        if (cueBallRb.linearVelocity.magnitude > 0.1f)
        {
            Vector3 targetPosition = cueBall.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            // Once the ball stops, snap camera position smoothly
            transform.position = Vector3.Lerp(transform.position, cueBall.position + offset, smoothSpeed * Time.deltaTime);
        }

        // Always look at the cue ball
        transform.LookAt(cueBall.position);
    }
}
