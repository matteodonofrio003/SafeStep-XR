using UnityEngine;

/// <summary>
/// Modulo di Wayfinding visivo per la simulazione industriale XR.
///
/// RESPONSABILITÀ SINGOLA: gestisce esclusivamente l'attivazione e la
/// disattivazione dei GameObject che rappresentano i percorsi a terra,
/// le basi di teletrasporto e gli indicatori luminosi che guidano 
/// l'operatore durante lo scenario.
/// 
/// COME COLLEGARE (Inspector di ScenarioManager):
///   OnEmergencyTriggered  →  VisualGuidanceSystem.ShowPathToDesk()
///   OnRobotIsolated       →  VisualGuidanceSystem.ShowPathToTank()
///   OnScenarioResolved    →  VisualGuidanceSystem.ShowPathToExit()
///   OnSlaFailed           →  VisualGuidanceSystem.HideAllPaths()
///
/// DIPENDENZE: nessuna. Non referenzia ScenarioManager, non legge stati.
/// Reagisce solo agli eventi ricevuti tramite UnityEvent.
/// </summary>
public class VisualGuidanceSystem : MonoBehaviour
{
    // =========================================================================
    // RIFERIMENTI AI PERCORSI — Assegnare nell'Inspector
    // =========================================================================

    [Header("Percorsi di Navigazione")]

    [Tooltip("GameObject che rappresenta il percorso a terra verso il pannello di controllo " +
             "/ scrivania (Desk). Attivato all'inizio del CascadeFailure.")]
    [SerializeField] private GameObject _prodPathToDesk;

    [Tooltip("GameObject che rappresenta il percorso a terra verso il reattore R-4 (Tank). " +
             "Attivato dopo che il robot è stato isolato.")]
    [SerializeField] private GameObject _prodPathToTank;

    [Tooltip("GameObject che rappresenta il percorso a terra verso l'uscita di sicurezza (Exit). " +
             "Attivato quando lo scenario è risolto con successo.")]
    [SerializeField] private GameObject _prodPathToExit;


    // =========================================================================
    // LIFECYCLE
    // =========================================================================

    private void Start()
    {
        // Stato iniziale pulito: nessun percorso visibile al caricamento della scena.
        HideAllPaths();
    }


    // =========================================================================
    // PUBLIC API — Chiamabili via UnityEvent dall'Inspector
    // =========================================================================

    /// <summary>
    /// Nasconde tutti i percorsi contemporaneamente.
    /// Chiamare in caso di reset, fallimento SLA, o fine simulazione.
    /// </summary>
    public void HideAllPaths()
    {
        SetPathActive(_prodPathToDesk, false);
        SetPathActive(_prodPathToTank, false);
        SetPathActive(_prodPathToExit, false);
    }

    /// <summary>
    /// Mostra il percorso verso il pannello di controllo (Desk).
    /// Collegare a: ScenarioManager.OnEmergencyTriggered
    /// Fase: l'operatore deve raggiungere il quadro per isolare il robot.
    /// </summary>
    public void ShowPathToDesk()
    {
        HideAllPaths();
        SetPathActive(_prodPathToDesk, true);

        Debug.Log("[VisualGuidanceSystem] 🟡 Percorso attivo: → Desk (pannello di controllo)");
    }

    /// <summary>
    /// Mostra il percorso verso il reattore R-4 (Tank).
    /// Collegare a: ScenarioManager.OnRobotIsolated
    /// Fase: robot isolato, ora l'operatore deve raggiungere il reattore per
    /// riavviare il raffreddamento prima che la temperatura superi la soglia critica.
    /// </summary>
    public void ShowPathToTank()
    {
        HideAllPaths();
        SetPathActive(_prodPathToTank, true);

        Debug.Log("[VisualGuidanceSystem] 🟠 Percorso attivo: → Tank (reattore R-4)");
    }

    /// <summary>
    /// Mostra il percorso verso l'uscita di sicurezza (Exit).
    /// Collegare a: ScenarioManager.OnScenarioResolved
    /// Fase: scenario risolto, guidare l'operatore fuori dall'area di pericolo.
    /// </summary>
    public void ShowPathToExit()
    {
        HideAllPaths();
        SetPathActive(_prodPathToExit, true);

        Debug.Log("[VisualGuidanceSystem] 🟢 Percorso attivo: → Exit (uscita sicurezza)");
    }


    // =========================================================================
    // UTILITY PRIVATA
    // =========================================================================

    /// <summary>
    /// Wrapper null-safe attorno a SetActive().
    /// Evita NullReferenceException se un percorso non è stato assegnato
    /// nell'Inspector durante lo sviluppo iterativo.
    /// </summary>
    /// <param name="path">Il GameObject del percorso da attivare/disattivare.</param>
    /// <param name="active">True per mostrare, false per nascondere.</param>
    private void SetPathActive(GameObject path, bool active)
    {
        if (path == null)
        {
            Debug.LogWarning($"[VisualGuidanceSystem] Riferimento mancante nell'Inspector " +
                             $"(tentativo di SetActive({active}) su un campo null).");
            return;
        }

        path.SetActive(active);
    }
}