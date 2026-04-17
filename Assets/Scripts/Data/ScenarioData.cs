using UnityEngine;

/// <summary>
/// ScriptableObject contenente tutti i parametri configurabili dello scenario di simulazione.
/// Separazione netta tra dati e logica: questo asset NON contiene alcun metodo funzionale.
/// Crea una nuova istanza via: Assets > Create > SafeStep-XR > Scenario Data
/// </summary>
[CreateAssetMenu(fileName = "NuovoScenarioETS", menuName = "SafeStep-XR/Scenario Data", order = 1)]
public class ScenarioDataSO : ScriptableObject
{
    // =========================================================================
    // GRUPPO 1: Robot Elettromeccanico
    // =========================================================================

    [Header("Robot Elettromeccanico")]

    [Tooltip("Corrente assorbita dal robot durante il funzionamento nominale (Ampere).")]
    [SerializeField] private float _assorbimentoNominale = 15f;

    [Tooltip("Corrente assorbita dal robot in condizione di stallo meccanico (Ampere). " +
             "Superare questa soglia indica un guasto critico.")]
    [SerializeField] private float _assorbimentoStallo = 65f;

    [Tooltip("Temperatura alla quale il surriscaldamento del motore diventa un rischio " +
             "concreto di incendio (gradi Celsius).")]
    [SerializeField] private float _temperaturaRischioIncendio = 130f;


    // =========================================================================
    // GRUPPO 2: Reattore R-4
    // =========================================================================

    [Header("Reattore R-4")]

    [Tooltip("Temperatura di esercizio nominale del reattore R-4 (gradi Celsius).")]
    [SerializeField] private float _temperaturaNominaleReattore = 65f;

    [Tooltip("Soglia critica: se raggiunta, il reattore R-4 va in esplosione (gradi Celsius).")]
    [SerializeField] private float _temperaturaCriticaEsplosione = 95f;

    // MODIFICA QUI: Aumentiamo a 15°C al minuto. 
    // In tempo reale (senza moltiplicatore), per fare +30°C ci metterà esattamente 2 minuti.
    [Tooltip("Velocità di incremento della temperatura del reattore R-4 (gradi Celsius al minuto).")]
    [SerializeField] private float _incrementoTemperaturaPerMinuto = 15f; 


    // =========================================================================
    // GRUPPO 3: SLA e Timer
    // =========================================================================

    [Header("SLA e Timer")]

    // MODIFICA QUI: Mettiamo 90 o 120 secondi (1 minuto e mezzo o 2 minuti reali)
    [Tooltip("Tempo massimo concesso al giocatore per completare lo scenario (secondi).")]
    [SerializeField] private float _limiteSlaSecondi = 120f; 

    // MODIFICA QUI CRITICA: Riporta questo valore a 1 per avere un tempo 1:1.
    [Tooltip("Fattore di accelerazione del timer. Valore 1 = tempo reale.")]
    [SerializeField] private float _moltiplicatoreDemo = 1f;


    // =========================================================================
    // PROPERTIES — Accesso in sola lettura dall'esterno (incapsulamento)
    // =========================================================================

    // --- Robot Elettromeccanico ---
    public float AssorbimentoNominale        => _assorbimentoNominale;
    public float AssorbimentoStallo          => _assorbimentoStallo;
    public float TemperaturaRischioIncendio  => _temperaturaRischioIncendio;

    // --- Reattore R-4 ---
    public float TemperaturaNominaleReattore      => _temperaturaNominaleReattore;
    public float TemperaturaCriticaEsplosione     => _temperaturaCriticaEsplosione;
    public float IncrementoTemperaturaPerMinuto   => _incrementoTemperaturaPerMinuto;

    // --- SLA e Timer ---
    public float LimiteSlaSecondi    => _limiteSlaSecondi;
    public float MoltiplicatoreDemo  => _moltiplicatoreDemo;
}