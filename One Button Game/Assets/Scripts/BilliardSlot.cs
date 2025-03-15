using UnityEngine;
using System.Collections;

public class BilliardSlot : MonoBehaviour
{
    public int score = 0; // Keep track of the score
    public CueStick cueStick; // Reference to the cue stick script

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BilliardBall")) // Check if the ball is a billiard ball
        {
            score++; // Increase score
            Destroy(other.gameObject); // Remove the ball
            
            StartCoroutine(WaitForBallsToStop()); // Wait before resetting
        }
    }

    IEnumerator WaitForBallsToStop()
    {
        // ✅ Ensure we wait for all balls to stop before resetting cue stick & camera
        yield return new WaitForSeconds(0.5f);

        while (AreBallsMoving() || IsCueBallMoving())
        {
            yield return null; // Keep waiting while balls are still moving
        }

        yield return new WaitForSeconds(0.5f); // Extra wait time for stability

        ResetCueStick(); // Now reset cue stick & camera
    }

    bool AreBallsMoving()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("BilliardBall");
        foreach (GameObject ball in balls)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null && rb.linearVelocity.magnitude > 0.1f)
            {
                return true; // At least one ball is still moving
            }
        }
        return false;
    }

    bool IsCueBallMoving()
    {
        GameObject cueBallObj = GameObject.FindWithTag("CueBall");
        if (cueBallObj != null)
        {
            Rigidbody cueBallRb = cueBallObj.GetComponent<Rigidbody>();
            return cueBallRb != null && cueBallRb.linearVelocity.magnitude > 0.1f;
        }
        return false;
    }

    void ResetCueStick()
    {
        GameObject cueBallObj = GameObject.FindWithTag("CueBall"); // Find the cue ball
        if (cueBallObj != null && cueStick != null)
        {
            Rigidbody cueBallRb = cueBallObj.GetComponent<Rigidbody>();
            if (cueBallRb != null)
            {
                cueStick.SetTarget(cueBallRb); // Reset cue stick to the cue ball
            }
        }
    }
}
