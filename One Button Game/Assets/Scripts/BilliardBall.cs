using UnityEngine;

public class BilliardBall : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError(name + " is missing a Rigidbody!");
        }
        else
        {
            FreezeBall(); // Keep frozen until actually hit
        }
    }

    public void Unfreeze()
    {
        if (rb != null)
        {
            rb.isKinematic = false; // Allow movement
        }
    }

    public void FreezeBall()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; // Keeps it still until hit
        }
    }

    // ✅ Only Unfreeze when physically hit by another moving object
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CueBall"))
        {
            Unfreeze();
        }
    }
}
