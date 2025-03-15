using UnityEngine;

public class CueStickCamera : MonoBehaviour
{
    public Transform firstPersonView; // Assign in Unity (Empty GameObject behind cue ball)
    public Vector3 cameraOffset = new Vector3(0, 0.5f, -0.5f); // Adjust for best POV

    void Update()
    {
        if (transform.parent != null) // Ensure it's attached to the cue stick
        {
            transform.localPosition = cameraOffset; // Maintain offset
            transform.localRotation = Quaternion.identity; // Prevent weird rotations
        }
    }

    public void ResetToFirstPersonView()
    {
        transform.position = firstPersonView.position;
        transform.rotation = firstPersonView.rotation;
    }
}
