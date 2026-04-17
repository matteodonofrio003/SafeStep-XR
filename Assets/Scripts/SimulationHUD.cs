using UnityEngine;
using TMPro; // Richiede TextMeshPro

/// <summary>
/// Gestisce l'interfaccia a schermo (HUD) in VR, inclusi timer e messaggi di stato.
/// </summary>
public class SimulationHUD : MonoBehaviour
{
    [Header("Riferimenti UI")]
    public TextMeshProUGUI testoIstruzioni;
    public TextMeshProUGUI testoTimer;

    [Header("Impostazioni Timer")]
    public float minutiIniziali = 2f;
    private float tempoResiduo;
    private bool timerAttivo = false;

    void Start()
    {
        tempoResiduo = minutiIniziali * 60f;
        AggiornaTimer();
        MostraMessaggio("Attesa avvio simulazione...");
    }

    void Update()
    {
        if (timerAttivo && tempoResiduo > 0)
        {
            tempoResiduo -= Time.deltaTime;
            AggiornaTimer();

            if (tempoResiduo <= 0)
            {
                tempoResiduo = 0;
                timerAttivo = false;
                // Supporta i tag HTML per i colori in TextMeshPro!
                MostraMessaggio("<color=red>TEMPO SCADUTO! Reattore compromesso.</color>");
            }
        }
    }

    // ================= API PUBBLICHE =================

    public void AvviaTimer()
    {
        timerAttivo = true;
    }

    public void FermaTimer()
    {
        timerAttivo = false;
    }

    /// <summary>
    /// Cambia il testo a schermo. Puoi inserire i messaggi direttamente dall'Inspector!
    /// Usa i tag come <color=red>Testo</color> per cambiare colore.
    /// </summary>
    public void MostraMessaggio(string nuovoMessaggio)
    {
        if (testoIstruzioni != null)
        {
            testoIstruzioni.text = nuovoMessaggio;
        }
    }

    private void AggiornaTimer()
    {
        if (testoTimer != null)
        {
            int minuti = Mathf.FloorToInt(tempoResiduo / 60);
            int secondi = Mathf.FloorToInt(tempoResiduo % 60);
            testoTimer.text = string.Format("Tempo Residuo: {0:00}:{1:00}", minuti, secondi);
        }
    }
}