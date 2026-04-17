using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton centrale che orchestra la macchina a stati dello scenario industriale.
/// Consuma i parametri fisici da ScenarioDataSO e notifica i sistemi esterni
/// tramite UnityEvents, mantenendo zero dipendenze dirette tra i sottosistemi.
/// </summary>
public class ScenarioManager : MonoBehaviour
{
    // =========================================================================
    // SINGLETON
    // =========================================================================

    public static ScenarioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[ScenarioManager] Istanza duplicata rilevata. Distruggo il duplicato.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // =========================================================================
    // STATO DELLO SCENARIO
    // =========================================================================

    /// <summary>
    /// Macchina a stati che rappresenta la fase corrente della simulazione industriale.
    /// </summary>
    public enum ScenarioState
    {
        /// <summary>Operatività normale. Nessuna anomalia attiva.</summary>
        NormalOperations,

        /// <summary>Guasto a cascata attivo. SLA timer e temperatura in crescita.</summary>
        CascadeFailure,

        /// <summary>Robot isolato dall'operatore. Emergenza elettrica fermata.</summary>
        RobotIsolated,

        /// <summary>Raffreddamento riattivato. Reattore in fase di stabilizzazione.</summary>
        ReactorStabilizing,

        /// <summary>SLA scaduto o temperatura critica raggiunta. Scenario fallito.</summary>
        SlaFailed
    }

    /// <summary>Stato corrente della simulazione. Accessibile in sola lettura dall'esterno.</summary>
    public ScenarioState CurrentState { get; private set; } = ScenarioState.NormalOperations;


    // =========================================================================
    // DIPENDENZE E CONFIGURAZIONE
    // =========================================================================

    [Header("Configurazione Scenario")]
    [Tooltip("Asset ScriptableObject contenente tutti i parametri fisici e i timer dello scenario.")]
    [SerializeField] private ScenarioDataSO scenarioData;


    // =========================================================================
    // STATO INTERNO — Tracciamento runtime
    // =========================================================================

    /// <summary>Tempo trascorso dall'inizio del CascadeFailure (secondi di gioco scalati).</summary>
    private float _currentSlaTimer;

    /// <summary>Temperatura corrente del reattore R-4 (gradi Celsius).</summary>
    private float _currentReactorTemp;

    /// <summary>Incremento temperatura al secondo, pre-calcolato in Start() per evitare divisioni in Update().</summary>
    private float _reactorTempIncreasePerSecond;


    // =========================================================================
    // UNITY EVENTS — Disaccoppiamento totale dai sottosistemi
    // =========================================================================

    [Header("Events")]
    [Tooltip("Invocato quando scatta il CascadeFailure. " +
             "Collegare: allarmi audio/visivi, UI di emergenza, haptic feedback VR.")]
    public UnityEvent OnEmergencyTriggered;

    [Tooltip("Invocato quando il timer SLA scade o la temperatura supera la soglia critica. " +
             "Collegare: schermata di fail, freeze della simulazione, log analytics.")]
    public UnityEvent OnSlaFailed;

    [Tooltip("Invocato quando l'operatore riavvia il raffreddamento e lo scenario è risolto. " +
             "Collegare: schermata di successo, calcolo score, replay summary.")]
    public UnityEvent OnScenarioResolved;

    [Tooltip("Invocato quando l'operatore isola correttamente il robot. " +
             "Collegare: VisualGuidanceSystem.ShowPathToTank() per attivare " +
             "il percorso verso il reattore R-4.")]
    public UnityEvent OnRobotIsolated;

    [Tooltip("Invocato quando l'operatore riavvia il raffreddamento. " +
         "Collegare: VisualGuidanceSystem.ShowPathToExit() e HUD.")]
    public UnityEvent OnCoolingRestarted;

    [Tooltip("Invocato quando l'operatore raggiunge l'uscita di emergenza.")]
    public UnityEvent OnExitReached;


    // =========================================================================
    // LIFECYCLE — Unity Messages
    // =========================================================================

    private void Start()
    {
        ValidateDependencies();

        CurrentState = ScenarioState.NormalOperations;

        // Inizializzazione stato fisico dal dato configurato nell'asset
        _currentReactorTemp = scenarioData.TemperaturaNominaleReattore;
        _currentSlaTimer    = 0f;

        // Pre-calcolo: converti l'incremento da °C/min a °C/sec una volta sola
        _reactorTempIncreasePerSecond = scenarioData.IncrementoTemperaturaPerMinuto / 60f;

        Debug.Log($"[ScenarioManager] Inizializzato. Temp reattore: {_currentReactorTemp}°C | " +
                  $"SLA limite: {scenarioData.LimiteSlaSecondi}s | " +
                  $"Demo x{scenarioData.MoltiplicatoreDemo}");
    }

    private void Update()
    {
    // Il timer si ferma solo se lo stato è SlaFailed o se lo scenario è risolto (stato finale)
    if (CurrentState == ScenarioState.CascadeFailure || 
        CurrentState == ScenarioState.RobotIsolated || 
        CurrentState == ScenarioState.ReactorStabilizing)
    {
        TickSlaTimer();
        
        // La temperatura invece potrebbe fermarsi se il raffreddamento è attivo
        if (CurrentState != ScenarioState.ReactorStabilizing)
        {
            TickReactorTemperature();
        }

        CheckFailureConditions();
    }
    }


    // =========================================================================
    // CORE LOGIC — Metodi privati di simulazione (chiamati solo da Update)
    // =========================================================================

    /// <summary>
    /// Avanza il timer SLA applicando il moltiplicatore demo.
    /// </summary>
    private void TickSlaTimer()
    {
        _currentSlaTimer += Time.deltaTime * scenarioData.MoltiplicatoreDemo;
    }

    /// <summary>
    /// Incrementa la temperatura del reattore per questo frame,
    /// scalata per il moltiplicatore demo.
    /// </summary>
    private void TickReactorTemperature()
    {
        _currentReactorTemp += _reactorTempIncreasePerSecond
                               * Time.deltaTime
                               * scenarioData.MoltiplicatoreDemo;
    }

    public string LastFailureReason { get; private set; }

    private void CheckFailureConditions()
    {
        float limite = scenarioData.LimiteSlaSecondi;
        bool slaScaduto = _currentSlaTimer >= limite;
        bool temperaturasCritica = _currentReactorTemp >= scenarioData.TemperaturaCriticaEsplosione;

        if (!slaScaduto && !temperaturasCritica) return;

        // Salviamo il motivo specifico
        LastFailureReason = slaScaduto 
            ? "TEMPO SCADUTO: Lo SLA è terminato!" 
            : "DISASTRO: Il reattore è esploso per la temperatura!";

        Debug.LogWarning($"[ScenarioManager] SCENARIO FALLITO — {LastFailureReason}");

        TransitionTo(ScenarioState.SlaFailed);
        OnSlaFailed?.Invoke(); // Questo attiverà il messaggio sull'HUD
    }

    // Metodo utile per l'HUD: restituisce il tempo RIMANENTE invece di quello trascorso
    public float GetRemainingSlaTime() 
    {
        return Mathf.Max(0, scenarioData.LimiteSlaSecondi - _currentSlaTimer);
    }

    // =========================================================================
    // PUBLIC API — Triggerabili dall'interazione VR (XR Interactables, ecc.)
    // =========================================================================

    /// <summary>
    /// Avvia il guasto a cascata. Chiamare da XRSimpleInteractable del pannello elettrico
    /// o da un trigger di scenario automatico.
    /// </summary>
    public void TriggerCascadeFailure()
    {
        if (CurrentState != ScenarioState.NormalOperations)
        {
            Debug.LogWarning($"[ScenarioManager] TriggerCascadeFailure ignorato — stato corrente: {CurrentState}");
            return;
        }

        Debug.Log("[ScenarioManager] ⚡ GUASTO A CASCATA ATTIVATO.");
        TransitionTo(ScenarioState.CascadeFailure);
        OnEmergencyTriggered?.Invoke();
    }

    /// <summary>
    /// L'operatore isola il robot. Ferma il rischio elettrico immediato,
    /// ma il reattore continua a scaldarsi finché non si riavvia il raffreddamento.
    /// </summary>
    public void IsolateRobot()
    {
        if (CurrentState != ScenarioState.CascadeFailure)
        {
            Debug.LogWarning($"[ScenarioManager] IsolateRobot ignorato — stato corrente: {CurrentState}");
            return;
        }

        Debug.Log("[ScenarioManager] 🔒 Robot isolato dall'operatore.");
        TransitionTo(ScenarioState.RobotIsolated);

        OnRobotIsolated?.Invoke();
    }

    /// <summary>
    /// L'operatore riavvia il sistema di raffreddamento del reattore R-4.
    /// </summary>
    public void RestartCooling()
    {
        // 1. GUARDIA SILENZIOSA
        if (CurrentState == ScenarioState.ReactorStabilizing) return; 

        // 2. CONTROLLO DI ERRORE VERO
        if (CurrentState != ScenarioState.RobotIsolated) {
            Debug.LogWarning($"[ScenarioManager] RestartCooling ignorato — stato corrente: {CurrentState}");
            return;
        }

        Debug.Log("[ScenarioManager] ❄️ Raffreddamento riavviato. Corri all'uscita!");
        TransitionTo(ScenarioState.ReactorStabilizing);

        // 3. LANCIA L'EVENTO!
        OnCoolingRestarted?.Invoke(); // <--- AGGIUNGI QUESTA RIGA
    }

    // Nuovo metodo da chiamare quando si tocca il box invisibile all'uscita
    public void ReachExit()
    {
        if (CurrentState != ScenarioState.ReactorStabilizing) 
        {
            Debug.LogWarning("Devi prima stabilizzare il reattore!");
            return;
        }

        Debug.Log("[ScenarioManager] 🏆 Uscita raggiunta! Timer fermato.");
        
        // Cambiamo stato in uno che non triggera l'Update (es. NormalOperations o uno nuovo)
        TransitionTo(ScenarioState.NormalOperations); 
        
        OnScenarioResolved?.Invoke(); // Qui scatta la vittoria
        OnExitReached?.Invoke();     // Apre la porta
    }

    
    // =========================================================================
    // UTILITY
    // =========================================================================

    /// <summary>
    /// Centralizza tutte le transizioni di stato per garantire logging uniforme
    /// e un unico punto di estensione futura (es. analytics, achievements).
    /// </summary>
    private void TransitionTo(ScenarioState newState)
    {
        Debug.Log($"[ScenarioManager] Stato: {CurrentState} → {newState}");
        CurrentState = newState;
    }

    /// <summary>
    /// Fail-fast in fase di sviluppo: esplode subito se manca il riferimento all'asset dati
    /// invece di generare NullReferenceException casuali in Update().
    /// </summary>
    private void ValidateDependencies()
    {
        if (scenarioData == null)
        {
            Debug.LogError("[ScenarioManager] ❌ scenarioData è NULL. " +
                           "Assegna un asset ScenarioDataSO nell'Inspector.");
            enabled = false;
        }
    }

    /// <summary>
    /// Espone i valori runtime per debug visivo (Inspector o HUD di sviluppo).
    /// </summary>
    public (float slaTimer, float reactorTemp) GetRuntimeDebugValues()
        => (_currentSlaTimer, _currentReactorTemp);
}