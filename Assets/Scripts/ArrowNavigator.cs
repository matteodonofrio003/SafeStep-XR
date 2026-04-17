using UnityEngine;

/// <summary>
/// Gestisce l'attivazione delle frecce di navigazione in base allo stato dell'emergenza.
/// Aggiungilo sullo stesso GameObject del ReactorManager.
/// </summary>
public class ArrowNavigator : MonoBehaviour
{
    [Header("Riferimento ReactorManager")]
    [SerializeField] private ReactorManager reactorManager;

    [Header("Frecce verso i DPI (attive quando parte l'emergenza)")]
    [SerializeField] private GameObject[] frecceVersoDP = new GameObject[3];

    [Header("Frecce verso la Valvola (attive dopo aver indossato i DPI)")]
    [SerializeField] private GameObject[] frecceVersoValvola = new GameObject[3];

    private bool _frecceValvolaAttivate = false;

    private void Awake()
    {
        if (reactorManager == null)
            reactorManager = GetComponent<ReactorManager>();

        // All'avvio tutte le frecce sono spente
        SetFrecce(frecceVersoDP, false);
        SetFrecce(frecceVersoValvola, false);
    }

    private void Update()
    {
        // Quando parte l'emergenza → accendi frecce DPI
        if (reactorManager.simulationStarted && !reactorManager.hasDPI && !_frecceValvolaAttivate)
        {
            SetFrecce(frecceVersoDP, true);
            SetFrecce(frecceVersoValvola, false);
        }

        // Quando il giocatore indossa i DPI → switcha le frecce
        if (reactorManager.hasDPI && !_frecceValvolaAttivate)
        {
            _frecceValvolaAttivate = true;
            SetFrecce(frecceVersoDP, false);
            SetFrecce(frecceVersoValvola, true);
        }

        // Quando l'emergenza è risolta → spegni tutto
        if (!reactorManager.isEmergencyActive && !reactorManager.simulationStarted && _frecceValvolaAttivate)
        {
            SetFrecce(frecceVersoDP, false);
            SetFrecce(frecceVersoValvola, false);
            _frecceValvolaAttivate = false;
        }
    }

    private void SetFrecce(GameObject[] frecce, bool attive)
    {
        foreach (var f in frecce)
        {
            if (f != null)
                f.SetActive(attive);
        }
    }

    /// <summary>
    /// Resetta lo stato delle frecce (chiamabile al riavvio della simulazione).
    /// </summary>
    public void Reset()
    {
        _frecceValvolaAttivate = false;
        SetFrecce(frecceVersoDP, false);
        SetFrecce(frecceVersoValvola, false);
    }
}