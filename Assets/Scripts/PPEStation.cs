using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// PPEStation — Stazione di vestizione DPI (Dispositivi di Protezione Individuale).
///
/// SETUP INSPECTOR:
///   • Assegna il GameObject che contiene ReactorManager nel campo "Reactor Manager"
///   • Collega OnDPIEquipped all'AudioSource per il suono di vestizione
///   • Collega OnDPIEquipped al metodo che fa sparire il GameObject 3D (es. SetActive false)
///   • Sul componente XR Simple Interactable dello stesso oggetto:
///       Select Entered → PPEStation.EquipDPI()
/// </summary>
public class PPEStation : MonoBehaviour
{
    // ----------------------------------------------------------
    // RIFERIMENTI
    // ----------------------------------------------------------
    [Header("Riferimenti")]
    [Tooltip("Il ReactorManager della scena. Verrà aggiornato alla vestizione.")]
    [SerializeField] private ReactorManager reactorManager;

    // ----------------------------------------------------------
    // EVENTI
    // ----------------------------------------------------------
    [Header("Eventi")]
    [Tooltip("Chiamato una volta sola quando i DPI vengono indossati. " +
             "Collegare: suono zip, disattivazione mesh 3D, feedback aptico, ecc.")]
    public UnityEvent OnDPIEquipped;

    // ----------------------------------------------------------
    // STATO INTERNO
    // ----------------------------------------------------------
    private bool _isEquipped = false;

    // ==========================================================
    // VALIDAZIONE IN EDITOR
    // ==========================================================
    private void Awake()
    {
        // Fallback: cerca il ReactorManager nella scena se non assegnato
        if (reactorManager == null)
        {
            reactorManager = FindObjectOfType<ReactorManager>();

            if (reactorManager == null)
                Debug.LogError("[PPEStation] ⚠️ ReactorManager non trovato nella scena! " +
                               "Assegnalo manualmente nell'Inspector.");
            else
                Debug.LogWarning("[PPEStation] ReactorManager trovato automaticamente via FindObjectOfType. " +
                                 "Assegnalo nell'Inspector per evitare overhead.");
        }
    }

    // ==========================================================
    // API PUBBLICA
    // ==========================================================

    /// <summary>
    /// Equipaggia i DPI sull'operatore.
    /// Collegare all'evento "Select Entered" dell'XR Simple Interactable.
    /// </summary>
    public void EquipDPI()
    {
        // Guard: impedisce doppia vestizione
        if (_isEquipped)
        {
            Debug.Log("[PPEStation] ℹ️ DPI già indossati. Azione ignorata.");
            return;
        }

        // Guard: sicurezza su riferimento mancante
        if (reactorManager == null)
        {
            Debug.LogError("[PPEStation] ❌ Impossibile equipaggiare: ReactorManager è null.");
            return;
        }

        // 1. Imposta il flag interno — nessuna doppia vestizione da ora
        _isEquipped = true;

        // 2. Notifica il ReactorManager: l'operatore è protetto
        reactorManager.hasDPI = true;

        // 3. Log di debug chiari
        Debug.Log("[PPEStation] ✅ DPI INDOSSATI — hasDPI impostato a TRUE sul ReactorManager.");
        Debug.Log("[PPEStation] 👷 L'operatore è ora autorizzato a interagire con il reattore in sicurezza.");

        // 4. Lancia l'evento verso l'esterno (suono, sparizione mesh, feedback aptico...)
        OnDPIEquipped?.Invoke();
    }

    /// <summary>
    /// Resetta lo stato della stazione (utile per riavviare lo scenario).
    /// </summary>
    public void ResetStation()
    {
        _isEquipped = false;

        if (reactorManager != null)
            reactorManager.hasDPI = false;

        Debug.Log("[PPEStation] 🔄 Stazione DPI resettata.");
    }
}