using UnityEngine;

/// <summary>
/// Sistema di allarme indipendente.
/// Gestisce sirena audio e lampeggio luci rosse.
/// Si collega a ReactorManager tramite UnityEvent (Inspector o codice),
/// senza nessun accoppiamento diretto tra le due classi.
///
/// SETUP INSPECTOR:
///   • Assegna un AudioSource con la clip della sirena in "Siren Source"
///   • Assegna i GameObject luce rossa in "Alarm Lights"
///   • ReactorManager.OnHighAlarmTriggered  → AlarmSystem.TriggerAlarm()
///   • ReactorManager.OnCriticalFailure     → AlarmSystem.TriggerAlarm()  (opzionale: intensità diversa)
///   • ReactorManager.ResolveEmergency      → AlarmSystem.StopAlarm()
/// </summary>
public class AlarmSystem : MonoBehaviour
{
    // ----------------------------------------------------------
    // RIFERIMENTI (assegnabili dall'Inspector)
    // ----------------------------------------------------------
    [Header("Audio")]
    [Tooltip("AudioSource con la clip della sirena. Loop consigliato = true.")]
    [SerializeField] private AudioSource sirenSource;

    [Header("Luci")]
    [Tooltip("Array di luci rosse da far lampeggiare.")]
    [SerializeField] private Light[] alarmLights;

    // ----------------------------------------------------------
    // PARAMETRI LAMPEGGIO
    // ----------------------------------------------------------
    [Header("Parametri Lampeggio")]
    [Tooltip("Intensità minima durante il lampeggio.")]
    [SerializeField] private float minIntensity  = 0f;

    [Tooltip("Intensità massima durante il lampeggio.")]
    [SerializeField] private float maxIntensity  = 4f;

    [Tooltip("Velocità del ciclo di lampeggio (Hz).")]
    [SerializeField] private float blinkSpeed    = 2f;

    // ----------------------------------------------------------
    // STATO INTERNO
    // ----------------------------------------------------------
    private bool isAlarmActive = false;

    // ==========================================================
    // UNITY LIFECYCLE
    // ==========================================================

    private void Awake()
    {
        // Sicurezza: parti con le luci spente
        SetLightsEnabled(false);
    }

    private void Update()
    {
        if (!isAlarmActive) return;

        // PingPong restituisce un valore che oscilla tra 0 e 1 nel tempo,
        // poi lo rimappiamo all'intervallo [minIntensity, maxIntensity]
        float t         = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

        foreach (Light light in alarmLights)
        {
            if (light != null)
                light.intensity = intensity;
        }
    }

    // ==========================================================
    // API PUBBLICA
    // ==========================================================

    /// <summary>
    /// Attiva sirena e lampeggio luci.
    /// Collegare a ReactorManager.OnHighAlarmTriggered e/o OnCriticalFailure.
    /// </summary>
    public void TriggerAlarm()
    {
        if (isAlarmActive) return; // già attivo, non fare nulla

        isAlarmActive = true;

        // Luci
        SetLightsEnabled(true);

        // Sirena
        if (sirenSource != null && !sirenSource.isPlaying)
        {
            sirenSource.loop = true;
            sirenSource.Play();
        }

        Debug.Log("🔴 AlarmSystem: Allarme ATTIVATO.");
    }

    /// <summary>
    /// Disattiva sirena e luci.
    /// Collegare a ReactorManager.ResolveEmergency o a un evento di reset.
    /// </summary>
    public void StopAlarm()
    {
        if (!isAlarmActive) return;

        isAlarmActive = false;

        // Sirena
        if (sirenSource != null && sirenSource.isPlaying)
            sirenSource.Stop();

        // Luci — spegni completamente
        SetLightsEnabled(false);

        Debug.Log("✅ AlarmSystem: Allarme DISATTIVATO.");
    }

    // ==========================================================
    // HELPER PRIVATI
    // ==========================================================

    private void SetLightsEnabled(bool enabled)
    {
        foreach (Light light in alarmLights)
        {
            if (light == null) continue;

            light.enabled   = enabled;
            light.intensity = enabled ? maxIntensity : 0f;
        }
    }
}