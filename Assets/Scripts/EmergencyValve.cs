using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// EmergencyValve — Valvola di sfogo d'emergenza con feedback audio XR.
/// </summary>
public class EmergencyValve : MonoBehaviour
{
    // ----------------------------------------------------------
    // RIFERIMENTI
    // ----------------------------------------------------------
    [Header("Riferimenti")]
    [Tooltip("Il ReactorManager della scena.")]
    [SerializeField] private ReactorManager reactorManager;

    [Tooltip("Riferimento al CustomValvolaVR per poterla resettare in caso di errore.")]
    [SerializeField] private CustomValvolaVR customValvola;

    // ----------------------------------------------------------
    // PARAMETRI VALVOLA
    // ----------------------------------------------------------
    [Header("Parametri Valvola")]
    [Tooltip("Valore normalizzato (0.0 -> 1.0) per attivare la chiusura.")]
    [SerializeField] [Range(0f, 1f)] private float closeThreshold = 0.95f;

    // ----------------------------------------------------------
    // FEEDBACK AUDIO MECCANICO
    // ----------------------------------------------------------
    [Header("Feedback Audio (Meccanico)")]
    [Tooltip("AudioSource per lo sfregamento metallico mentre si gira la valvola.")]
    [SerializeField] private AudioSource mechanicalAudioSource;

    [Tooltip("Soglia minima di movimento per ignorare il tremolio della mano in VR.")]
    [SerializeField] private float rotationJitterThreshold = 0.005f;

    [Tooltip("Secondi di inattività prima che l'audio si fermi.")]
    [SerializeField] private float audioStopDelay = 0.15f;

    // ----------------------------------------------------------
    // EVENTI
    // ----------------------------------------------------------
    [Header("Eventi")]
    [Tooltip("Chiamato quando la valvola è chiusa con successo.")]
    public UnityEvent OnValveClosed;

    [Tooltip("Chiamato se l'utente prova a chiudere senza i DPI.")]
    public UnityEvent OnValveFailed;

    // ----------------------------------------------------------
    // STATO INTERNO
    // ----------------------------------------------------------
    private bool _isResolved = false;
    private float _lastRotationValue = 0f;
    private float _timeSinceLastRotation = 0f;
    private bool _isBeingHeld = false;

    // ==========================================================
    // LIFECYCLE
    // ==========================================================
    private void Awake()
    {
        if (reactorManager == null)
            reactorManager = FindObjectOfType<ReactorManager>();

        if (reactorManager == null)
            Debug.LogError("[EmergencyValve] ⚠️ ReactorManager non trovato!");

        if (customValvola == null)
            customValvola = GetComponent<CustomValvolaVR>();

        if (mechanicalAudioSource != null)
        {
            mechanicalAudioSource.loop = true;
            mechanicalAudioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        // Gestione stop audio dopo inattività
        if (mechanicalAudioSource != null && mechanicalAudioSource.isPlaying)
        {
            _timeSinceLastRotation += Time.deltaTime;

            if (_timeSinceLastRotation > audioStopDelay)
            {
                mechanicalAudioSource.Pause();
            }
        }
    }

    // ==========================================================
    // API PUBBLICA
    // ==========================================================

    /// <summary>
    /// Chiamato da CustomValvolaVR tramite OnValueChange (range 0.0 – 1.0).
    /// Collega questo metodo nel campo "On Value Change" del CustomValvolaVR.
    /// </summary>
    public void OnValveRotated(float rotationValue)
    {
        if (_isResolved) return;

        Debug.Log($"[EmergencyValve] OnValveRotated: {rotationValue:F3}");

        // 1. Audio meccanico
        GestisciAudioRotazione(rotationValue);

        // 2. Logica chiusura
        if (rotationValue >= closeThreshold)
        {
            TentativoChiusura();
        }
    }

    /// <summary>
    /// Chiamato dall'XR Simple Interactable → OnSelectEntered.
    /// Serve a sapere quando il giocatore afferra la valvola.
    /// </summary>
    public void OnValveGrabbed()
    {
        _isBeingHeld = true;
        _lastRotationValue = 0f; // Reset per evitare delta falso al primo frame
        _timeSinceLastRotation = 0f;
        Debug.Log("[EmergencyValve] Valvola afferrata.");
    }

    /// <summary>
    /// Chiamato dall'XR Simple Interactable → OnSelectExited.
    /// Ferma l'audio quando il giocatore rilascia la valvola.
    /// </summary>
    public void OnValveReleased()
    {
        _isBeingHeld = false;

        if (mechanicalAudioSource != null)
            mechanicalAudioSource.Pause();

        Debug.Log("[EmergencyValve] Valvola rilasciata.");
    }

    // ==========================================================
    // METODI PRIVATI
    // ==========================================================

    private void GestisciAudioRotazione(float currentRotation)
    {
        if (mechanicalAudioSource == null) return;

        float delta = Mathf.Abs(currentRotation - _lastRotationValue);

        if (delta > rotationJitterThreshold)
        {
            _timeSinceLastRotation = 0f;

            if (!mechanicalAudioSource.isPlaying)
            {
                mechanicalAudioSource.UnPause();
                mechanicalAudioSource.Play();
            }
        }

        _lastRotationValue = currentRotation;
    }

    private void TentativoChiusura()
    {
        if (reactorManager == null) return;

        bool successo = reactorManager.ResolveEmergency();

        if (successo)
        {
            _isResolved = true;

            if (mechanicalAudioSource != null)
                mechanicalAudioSource.Stop();

            OnValveClosed?.Invoke();
            Debug.Log("[EmergencyValve] ✅ Procedura completata con successo.");
        }
        else
        {
            // Reset valvola visiva e logica
            if (customValvola != null)
                customValvola.ResetRotation();

            _lastRotationValue = 0f;

            if (mechanicalAudioSource != null)
                mechanicalAudioSource.Pause();

            OnValveFailed?.Invoke();
            Debug.LogWarning("[EmergencyValve] ❌ DPI mancanti! Errore comunicato all'utente.");
        }
    }

    /// <summary>
    /// Resetta completamente la valvola (es. al riavvio della simulazione).
    /// </summary>
    public void ResetValve()
    {
        _isResolved = false;
        _lastRotationValue = 0f;
        _isBeingHeld = false;

        if (mechanicalAudioSource != null)
            mechanicalAudioSource.Stop();

        if (customValvola != null)
            customValvola.ResetRotation();

        Debug.Log("[EmergencyValve] 🔄 Valvola resettata.");
    }
}