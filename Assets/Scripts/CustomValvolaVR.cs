using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Simulatore di Valvola VR custom. 
/// Ruota in base alla posizione della mano dell'operatore e restituisce un valore da 0 a 1.
/// </summary>
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))] 
public class CustomValvolaVR : MonoBehaviour
{
    [Header("Impostazioni Valvola")]
    [Tooltip("L'asse attorno a cui ruota la valvola (di solito Z o Y, a seconda di come è stato importato il 3D).")]
    public Vector3 asseDiRotazione = Vector3.forward; // (0, 0, 1)

    [Tooltip("Quanti gradi devi girare per chiuderla completamente (es. 360 = 1 giro completo).")]
    public float gradiPerChiusura = 360f;

    [Header("Evento di Uscita")]
    [Tooltip("Invia il valore da 0.0 (aperta) a 1.0 (chiusa). Collegare a EmergencyValve.OnValveRotated")]
    public UnityEvent<float> OnValueChange;

    // Componenti e stato interno
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable _interactable;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor _interactor;
    private float _rotazioneAttuale = 0f;
    private Vector3 _ultimaPosizioneMano;

    void Awake()
    {
        _interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        
        // Colleghiamo i nostri metodi agli eventi di Grab e Release del VR
        _interactable.selectEntered.AddListener(OnGrab);
        _interactable.selectExited.AddListener(OnRelease);
    }

    void OnDestroy()
    {
        _interactable.selectEntered.RemoveListener(OnGrab);
        _interactable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        _interactor = args.interactorObject; // Salviamo quale mano ci sta afferrando
        
        // Salviamo la posizione di partenza della mano rispetto al centro della valvola
        _ultimaPosizioneMano = transform.InverseTransformPoint(_interactor.transform.position);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        _interactor = null; // Sganciamo la mano
    }

    void Update()
    {
        // Se qualcuno sta tenendo la valvola...
        if (_interactor != null)
        {
            // Trova dove si trova la mano adesso
            Vector3 posizioneAttualeMano = transform.InverseTransformPoint(_interactor.transform.position);

            // Proiettiamo queste posizioni su un piano piatto 2D (come disegnare su un foglio)
            Vector3 attualeSuPiano = Vector3.ProjectOnPlane(posizioneAttualeMano, asseDiRotazione).normalized;
            Vector3 ultimaSuPiano = Vector3.ProjectOnPlane(_ultimaPosizioneMano, asseDiRotazione).normalized;

            // 1. PRIMA CALCOLA L'ANGOLO DI SCARTO TRA I DUE FRAME
            float angoloDiScarto = Vector3.SignedAngle(ultimaSuPiano, attualeSuPiano, asseDiRotazione);

            // 2. TRUCCO HACKATHON: SOMMA QUALSIASI MOVIMENTO USANDO IL VALORE ASSOLUTO
            _rotazioneAttuale += Mathf.Abs(angoloDiScarto);

            // 3. Blocca la rotazione in modo che non vada sotto 0 o sopra i gradi massimi
            _rotazioneAttuale = Mathf.Clamp(_rotazioneAttuale, 0f, gradiPerChiusura);

            // 4. RUOTA IL MODELLO 3D
            transform.localRotation = Quaternion.AngleAxis(_rotazioneAttuale, asseDiRotazione);

            // 5. INVIA IL VALORE ALLO SCRIPT DI EMERGENZA (Normalizzato tra 0.0 e 1.0)
            float valoreNormalizzato = _rotazioneAttuale / gradiPerChiusura;
            
            // Stampa di sicurezza per vedere che i numeri salgano sempre
            // Debug.Log($"[RADAR VALVOLA] Valore inviato: {valoreNormalizzato}");
            
            OnValueChange?.Invoke(valoreNormalizzato);

            // Aggiorna per il prossimo frame
            _ultimaPosizioneMano = posizioneAttualeMano;
        }
    }

    // ==========================================================
    // METODO DI RESET (Aggiunto per penalità DPI mancanti)
    // ==========================================================
    
    /// <summary>
    /// Azzera la rotazione della valvola e la fa scattare visivamente alla posizione iniziale.
    /// Viene chiamato dall'EmergencyValve se l'operatore prova a chiudere senza DPI.
    /// </summary>
    public void ResetRotation()
    {
        // 1. Azzera il calcolo interno
        _rotazioneAttuale = 0f;
        
        // 2. Resetta visivamente il modello 3D alla rotazione base (0,0,0)
        transform.localRotation = Quaternion.identity; 
        
        // 3. Avvisa il sistema che la valvola è tornata a zero
        OnValueChange?.Invoke(0f);
        
        Debug.Log("[CustomValvolaVR] 🔄 Rotazione azzerata. L'operatore deve riprovare da capo.");
    }
}