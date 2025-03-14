using UnityEngine;

public class CueStick : MonoBehaviour
{
    public Rigidbody cueBall; // Assign the cue ball in Inspector
    public float minForce = 5f;  // Weakest hit
    public float maxForce = 30f; // Strongest hit
    public float chargeSpeed = 10f; // How fast power increases

    private bool isCharging = false;
    private float currentPower;

    void Update()
    {
        // Start charging when button is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            currentPower = minForce; // Start at min force
        }

        // Increase power while holding the button
        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            currentPower += chargeSpeed * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, minForce, maxForce); // Limit power
        }

        // Release shot
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            HitCueBall();
            isCharging = false;
        }
    }

    void HitCueBall()
    {
        // Direction to shoot (forward from cue stick)
        Vector3 shootDirection = transform.forward;

        // Apply force to cue ball
        cueBall.AddForce(shootDirection * currentPower, ForceMode.Impulse);
    }
}
