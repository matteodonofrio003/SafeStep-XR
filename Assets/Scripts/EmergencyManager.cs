using UnityEngine;
using TMPro;

public class EmergencyManager : MonoBehaviour
{
    [Header("Parametri Base")]
    public float timeRemaining = 30f;
    public bool isEmergencyActive = false;
    public bool isMachineSafe = false;
    public TMP_Text timerText;

    [Header("Effetti Allarme")]
    public Light emergencyLight;
    public AudioSource sirenSound;

    [Header("Sistema di Guida")]
    public GameObject buttonHighlight; // La luce sul pulsante rosso
    public GameObject pathToMachine;   // Frecce verso il macchinario
    public GameObject pathToExit;      // Frecce verso la porta verde

    [Header("Animazione Uscita")]
    public DoorOpener exitDoor;        // Trascina qui la porta che ha lo script DoorOpener

    void Start()
    {
        // Al caricamento della scena, nascondiamo TUTTO
        if (emergencyLight != null) emergencyLight.enabled = false;
        if (sirenSound != null) sirenSound.Stop();
        
        // Disattiva tutti gli aiuti visivi
        SetGuidance(false, false, false);
        
        Debug.Log("Sistema pronto. Premi il pulsante verde per iniziare.");
    }

    void Update()
    {
        if (isEmergencyActive)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
                
                // La luce rossa lampeggia finché l'emergenza è attiva
                if (emergencyLight != null && emergencyLight.enabled) 
                    emergencyLight.intensity = Mathf.PingPong(Time.time * 10, 15);
            }
            else
            {
                GameOver();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        timerText.text = "TEMPO: " + timeRemaining.ToString("F1");
    }

    void GameOver()
    {
        isEmergencyActive = false;
        timeRemaining = 0;
        timerText.text = "MISSIONE FALLITA!";
        timerText.color = Color.red;
        
        // In caso di fallimento spegniamo tutto per sicurezza
        if (sirenSound != null) sirenSound.Stop();
        if (emergencyLight != null) emergencyLight.enabled = false;
        SetGuidance(false, false, false);
    }

    public void StartEmergency()
    {
        isEmergencyActive = true;
        isMachineSafe = false;
        timeRemaining = 30f;
        timerText.color = Color.white;
        
        // ATTIVIAMO ALLARME (Luce e Suono)
        if (emergencyLight != null) emergencyLight.enabled = true;
        if (sirenSound != null) sirenSound.Play();

        // GUIDA: Mostra SOLO il percorso verso il pulsante rosso
        SetGuidance(true, true, false);
        
        Debug.Log("Emergenza Iniziata! Vai al macchinario.");
    }

    public void FixMachine()
    {
        if (isEmergencyActive)
        {
            isMachineSafe = true;
            timerText.text = "CORRI ALL'USCITA!";
            timerText.color = Color.yellow;
            
            // Cambiamo solo la guida visiva:
            SetGuidance(false, false, true);
            
            Debug.Log("Macchina sicura. Allarme ancora attivo, scappa!");
        }
    }

    public void ReachExit()
    {
        // Il successo avviene SOLO se l'emergenza è attiva E la macchina è sicura
        if (isEmergencyActive && isMachineSafe)
        {
            isEmergencyActive = false;
            timerText.text = "SUCCESSO!";
            timerText.color = Color.green;

            // ORA spegniamo tutto perché l'utente è fuori pericolo
            if (sirenSound != null) sirenSound.Stop();
            if (emergencyLight != null) emergencyLight.enabled = false;

            // Nascondiamo l'ultimo percorso rimasto
            SetGuidance(false, false, false);

            // APERTURA FISICA DELLA PORTA
            if (exitDoor != null)
            {
                exitDoor.OpenDoor();
            }
            
            Debug.Log("Simulazione completata con successo! Porta aperta.");
        }
        else if (isEmergencyActive && !isMachineSafe)
        {
            timerText.text = "TORNA INDIETRO! SICUREZZA PRIMA!";
            Debug.Log("Tentativo di uscita fallito: rubinetto non chiuso.");
        }
    }

    // Funzione helper per gestire la visibilità degli oggetti di guida
    void SetGuidance(bool highlight, bool toMachine, bool toExit)
    {
        if (buttonHighlight != null) buttonHighlight.SetActive(highlight);
        if (pathToMachine != null) pathToMachine.SetActive(toMachine);
        if (pathToExit != null) pathToExit.SetActive(toExit);
    }
}