using UnityEngine;
using System.Collections;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance { get; private set; }

    [Header("Riferimenti Player")]
    public Transform playerTransform;
    public CharacterController characterController; // opzionale
    public Rigidbody playerRigidbody;              // opzionale

    [Header("Effetto")]
    public float fadeDuration = 0.3f;
    public bool useFade = true;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void TeleportTo(TeleportDestination destination)
    {
        if (destination == null) return;
        StartCoroutine(DoTeleport(destination.GetPosition(), destination.GetRotation()));
    }

    public void TeleportTo(Vector3 position, Quaternion rotation)
    {
        StartCoroutine(DoTeleport(position, rotation));
    }

    private IEnumerator DoTeleport(Vector3 targetPos, Quaternion targetRot)
    {
        if (characterController != null) characterController.enabled = false;
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        if (useFade) yield return StartCoroutine(FadeOut());

        // Trova la camera principale dentro l'XR Origin
        Camera xrCamera = playerTransform.GetComponentInChildren<Camera>();

        if (xrCamera != null)
        {
            // Calcola l'offset orizzontale tra camera e origin (movimento fisico/joystick)
            Vector3 cameraOffset = xrCamera.transform.position - playerTransform.position;
            cameraOffset.y = 0; // ignora l'altezza

            // Sposta l'origin compensando l'offset
            playerTransform.position = targetPos - cameraOffset;
        }
        else
        {
            // Fallback senza XR
            playerTransform.position = targetPos;
        }

        playerTransform.rotation = targetRot;

        if (characterController != null) characterController.enabled = true;

        if (useFade) yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        // Aggiungi qui il tuo sistema di fade (CanvasGroup, post-process, ecc.)
        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(fadeDuration);
    }
}