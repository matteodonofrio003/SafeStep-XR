using UnityEngine;

public class TeleportDestination : MonoBehaviour
{
    [Header("Destinazione")]
    public string locationName = "Nuova Destinazione";
    public Sprite icon; // opzionale, per il bottone

    public Vector3 GetPosition() => transform.position;
    public Quaternion GetRotation() => transform.rotation;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
    }
}