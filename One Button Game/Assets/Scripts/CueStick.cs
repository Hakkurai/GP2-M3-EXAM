using UnityEngine;
using System.Collections;

public class CueStick : MonoBehaviour
{
    public Rigidbody cueBall; // Reference to the cue ball
    public float minForce = 2f;
    public float maxForce = 20f;
    public float chargeSpeed = 15f;
    public float pullBackDistance = 0.5f; // Increased for better visual effect
    public float shotSpeed = 20f; // Faster movement for realism
    public Transform cueTip; // The actual tip of the cue stick

    private bool isCharging = false;
    private float currentPower;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        // Prevent shooting if the cue ball is still moving
        if (cueBall.linearVelocity.magnitude > 0.1f)
            return;

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
            transform.localPosition = originalPosition - transform.forward * (pullBackDistance * (currentPower / maxForce));
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            StartCoroutine(Shoot());
            isCharging = false;
        }
    }

    IEnumerator Shoot()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = originalPosition; // Moves forward to hit

        float elapsedTime = 0f;
        float hitSpeed = 0.05f; // Fast push forward

        while (elapsedTime < hitSpeed)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, (elapsedTime / hitSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;

        ApplyForce();
    }

    void ApplyForce()
    {
        // Ensure cue stick is close enough before applying force
        float distance = Vector3.Distance(cueTip.position, cueBall.position);
        if (distance > 0.5f)
        {
            Debug.Log("Cue stick is too far! No contact.");
            return;
        }

        // Unfreeze all balls after the first shot
        GameObject[] balls = GameObject.FindGameObjectsWithTag("BilliardBall");
        foreach (GameObject ball in balls)
        {
            ball.GetComponent<BilliardBall>().Unfreeze();
        }

        cueBall.AddForce(transform.forward * currentPower, ForceMode.Impulse);

        // 🔥 Reset camera & cue stick after shooting
        FindObjectOfType<CueStickCamera>().ResetToFirstPersonView();
    }
}
