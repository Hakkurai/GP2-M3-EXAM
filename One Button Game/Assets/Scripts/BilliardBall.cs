using UnityEngine;

public class BilliardBall : MonoBehaviour
{
    private Rigidbody rb;
    private float stopThreshold = 0.05f; // Speed below which the ball will stop

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ?? Freeze balls 1-15 at start, unfreeze only when first shot happens
        if (gameObject.CompareTag("BilliardBall"))
        {
            rb.isKinematic = true; // Prevents any movement before break
        }

        // Make rolling more realistic
        rb.linearDamping = 0.4f;           // Controls linear stopping
        rb.angularDamping = 0.5f;    // Controls rotation stopping
    }

    void Update()
    {
        // Auto-stop rolling if speed is too low
        if (rb.linearVelocity.magnitude < stopThreshold && rb.angularVelocity.magnitude < stopThreshold)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // Call this when the first shot happens to unfreeze balls
    public void Unfreeze()
    {
        rb.isKinematic = false; // Balls can now move
    }
}
