using UnityEngine;
using UnityEngine.Events;

// ===========================================================
// Evento custom tipizzato per passare due float (pressione, temperatura)
// Definito fuori dalla classe per essere riutilizzabile nel progetto
// ===========================================================
[System.Serializable]
public class TelemetryEvent : UnityEvent<float, float> { }

public class ReactorManager : MonoBehaviour
{
    // ----------------------------------------------------------
    // EVENTI PUBBLICI (collegabili dall'Inspector o via codice)
    // ----------------------------------------------------------
    [Header("Eventi Sistema")]
    public UnityEvent OnHighAlarmTriggered;   // Scatta a >= 4.5 bar (una volta sola)
    public UnityEvent OnCriticalFailure;      // Scatta a >= 8.5 bar
    public TelemetryEvent OnTelemetryUpdated; // Scatta ad ogni tick: passa (pressione, temperatura)

    // ----------------------------------------------------------
    // STATO DEL REATTORE (leggibile da script esterni)
    // ----------------------------------------------------------
    [Header("Parametri in Tempo Reale")]
    public float currentPressure   = 2.5f;
    public float currentTemp       = 65.0f;
    public bool  simulationStarted = false;
    public bool  hasDPI            = false;
    public bool  isEmergencyActive = true;

    // ----------------------------------------------------------
    // SOGLIE
    // ----------------------------------------------------------
    [Header("Soglie (Da Documentazione)")]
    public float highAlarmThreshold      = 4.5f;
    public float criticalFailureThreshold = 8.5f;

    // ----------------------------------------------------------
    // IMPOSTAZIONI TIMER
    // ----------------------------------------------------------
    [Header("Impostazioni Timer")]
    public float timeInterval    = 5.0f;  // secondi tra ogni incremento
    public float pressureIncrease = 0.5f; // bar per tick

    // ----------------------------------------------------------
    // STATO INTERNO (privato)
    // ----------------------------------------------------------
    private float timer            = 0f;
    private bool  isAlarmTriggered = false;

    // ==========================================================
    // UPDATE LOOP
    // ==========================================================
    void Update()
    {
        // La simulazione gira solo se avviata e l'emergenza è attiva
        if (!simulationStarted || !isEmergencyActive) return;

        timer += Time.deltaTime;

        if (timer >= timeInterval)
        {
            timer = 0f;

            // 1. Incrementa la pressione
            currentPressure += pressureIncrease;

            // 2. Notifica le UI esterne con i nuovi valori (disaccoppiato)
            OnTelemetryUpdated?.Invoke(currentPressure, currentTemp);

            // 3. Controlla le soglie critiche
            CheckStatus();
        }
    }

    // ==========================================================
    // LOGICA DI CONTROLLO SOGLIE
    // ==========================================================
    private void CheckStatus()
    {
        // Fallimento critico ha priorità assoluta
        if (currentPressure >= criticalFailureThreshold)
        {
            TriggerCriticalFailure();
            return;
        }

        // Allarme alto (scatta una volta sola)
        if (currentPressure >= highAlarmThreshold && !isAlarmTriggered)
        {
            isAlarmTriggered = true;
            Debug.LogWarning("🚨 ALLARME SCATTATO! Pressione a " + currentPressure + " bar!");
            OnHighAlarmTriggered?.Invoke();
        }
    }

    private void TriggerCriticalFailure()
    {
        isEmergencyActive  = false;
        simulationStarted  = false;

        Debug.LogError("💥 FALLIMENTO CRITICO! Il reattore ha ceduto a " + currentPressure + " bar!");
        OnCriticalFailure?.Invoke();
    }

    // ==========================================================
    // API PUBBLICA — chiamabile da altri script / XR Interactable
    // ==========================================================

    /// <summary>Avvia la simulazione dell'emergenza pneumatica.</summary>
    public void StartSimulation()
    {
        simulationStarted  = true;
        isEmergencyActive  = true;
        isAlarmTriggered   = false;
        Debug.Log("⚠️ EMERGENZA INIZIATA: Guasto pneumatico rilevato!");
    }

    /// <summary>
    /// Chiamato quando il giocatore gira la valvola di sfogo.
    /// Risolve l'emergenza con o senza penalità DPI.
    /// </summary>
// Cambiamo da 'public void' a 'public bool'
public bool ResolveEmergency() 
{
    if (hasDPI)
    {
        isEmergencyActive = false;
        simulationStarted = false;
        currentPressure = 2.0f;
        OnTelemetryUpdated?.Invoke(currentPressure, currentTemp);
        
        Debug.Log("✅ EMERGENZA RISOLTA IN SICUREZZA!");
        return true; // Ritorna VERO: tutto ok!
    }
    else
    {
        Debug.LogError("❌ INFORTUNIO! Hai operato la valvola senza DPI!");
        // Non resettiamo la pressione, l'emergenza resta attiva
        return false; // Ritorna FALSO: operazione annullata!
    }
}
}