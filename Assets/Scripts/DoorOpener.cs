using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public float openAngle = 90f; 
    public float smoothSpeed = 2f; 
    private bool isOpening = false;
    private Quaternion targetRotation;
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        if (isOpening)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
    }
}
