using UnityEngine;
using System.Collections;

public class CueStick : MonoBehaviour
{
    public Rigidbody cueBall; // The white cue ball
    public float minForce = 2f;
    public float maxForce = 20f;
    public float chargeSpeed = 15f;
    public float pullBackDistance = 0.5f;
    public float rotationSpeed = 100f; // Speed of rotation around CueBall
    public float cueStickDistance = 1.5f; // How far behind the CueBall the cue stick should reset
    public float cameraHeight = 1.5f; // Camera height offset
    public float cameraDistance = 2f; // Camera distance from cue ball
    public Transform cameraTransform; // Reference to the camera

    private bool isCharging = false;
    private float currentPower;
    private bool isWaitingForBallsToStop = false;

    void Start()
    {
        PositionCueStickNearCueBall();
    }

    void Update()
    {
        if (isWaitingForBallsToStop) return;

        RotateCueStickWithMouse();

        if (!isCharging && Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            currentPower = minForce;
        }

        if (isCharging && Input.GetKey(KeyCode.Space))
        {
            currentPower += chargeSpeed * Time.deltaTime;
            currentPower = Mathf.Clamp(currentPower, minForce, maxForce);

            // Move cue stick back smoothly while charging
            transform.position -= transform.forward * (pullBackDistance * (currentPower / maxForce));
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            StartCoroutine(Shoot());
            isCharging = false;
        }
    }

    void RotateCueStickWithMouse()
    {
        if (cueBall == null) return;

        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        // ✅ Rotate the cue stick around the cue ball (horizontal movement)
        transform.RotateAround(cueBall.position, Vector3.up, mouseX);

        // ✅ Ensure cue stick is looking at cue ball
        transform.LookAt(cueBall.position);
    }

    IEnumerator Shoot()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = cueBall.position; // Moves forward to hit

        float elapsedTime = 0f;
        float hitSpeed = 0.05f;

        while (elapsedTime < hitSpeed)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / hitSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = cueBall.position;

        ApplyForce();
    }
void ApplyForce()
{
    if (cueBall == null) return;

    float distance = Vector3.Distance(transform.position, cueBall.position);
    if (distance > 0.5f)
    {
        Debug.Log("Cue stick is too far! No contact.");
        return;
    }

    // ✅ Apply strong force so cue ball transfers enough energy
    Vector3 shotDirection = transform.forward;

    Rigidbody cueBallRb = cueBall.GetComponent<Rigidbody>();
    if (cueBallRb != null)
    {
        cueBallRb.AddForce(shotDirection * currentPower * 3f, ForceMode.Impulse);
    }
    else
    {
        Debug.LogError("CueBall Rigidbody missing!");
    }

    StartCoroutine(WaitForBallsToStop());
}


    IEnumerator WaitForBallsToStop()
    {
        isWaitingForBallsToStop = true;

        yield return new WaitForSeconds(0.5f);

        while (AreBallsMoving() || IsCueBallMoving())
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        isWaitingForBallsToStop = false;
        PositionCueStickNearCueBall();
    }

    bool AreBallsMoving()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("BilliardBall");
        foreach (GameObject ball in balls)
        {
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null && rb.linearVelocity.magnitude > 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    bool IsCueBallMoving()
    {
        return cueBall != null && cueBall.linearVelocity.magnitude > 0.1f;
    }

    void PositionCueStickNearCueBall()
    {
        if (cueBall == null) return;

        // ✅ FORCIBLY MOVE THE CUE STICK BEHIND THE CUE BALL
        Vector3 cueStickOffset = new Vector3(0f, 0f, -cueStickDistance); // Force it backward on the Z-axis
        transform.position = cueBall.position + cueStickOffset; // Move it behind the cue ball

        // ✅ Ensure it stays at the same height as the cue ball
        transform.position = new Vector3(transform.position.x, cueBall.position.y, transform.position.z);

        // ✅ Force cue stick to face the cue ball
        transform.LookAt(cueBall.position);

        // ✅ Adjust the camera position (Behind & Above)
        if (cameraTransform != null)
        {
            Vector3 cameraOffset = new Vector3(0f, cameraHeight, -cameraDistance);
            cameraTransform.position = cueBall.position + cameraOffset;
            cameraTransform.LookAt(cueBall.position);
        }
    }

    public void SetTarget(Rigidbody newCueBall)
    {
        cueBall = newCueBall;
        PositionCueStickNearCueBall();
    }
}
