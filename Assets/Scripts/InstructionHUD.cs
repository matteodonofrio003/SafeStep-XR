using UnityEngine;
using TMPro;

public class InstructionHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _instructionText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("VR Smoothing")]
    [SerializeField, Range(1f, 15f)] private float _rotationSmoothSpeed = 8f;

    private Camera _vrCamera;

    private void Start()
    {
        if (_instructionText == null || _timerText == null) return;

        _instructionText.text = string.Empty;
        _timerText.text = string.Empty;
        _vrCamera = Camera.main;
    }

    private void Update()
    {
        if (_vrCamera == null) return;

        UpdateLiveTimer();

        Vector3 directionFromCamera = transform.position - _vrCamera.transform.position;
        if (directionFromCamera.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionFromCamera);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSmoothSpeed);
        }
    }

    private void UpdateLiveTimer()
    {
        if (_timerText == null) return;
        var scenario = ScenarioManager.Instance;
        if (scenario == null) return;

        bool isTimerRunning = (scenario.CurrentState == ScenarioManager.ScenarioState.CascadeFailure || 
                               scenario.CurrentState == ScenarioManager.ScenarioState.RobotIsolated ||
                               scenario.CurrentState == ScenarioManager.ScenarioState.ReactorStabilizing);

        if (!isTimerRunning && scenario.CurrentState != ScenarioManager.ScenarioState.SlaFailed)
        {
            _timerText.text = string.Empty;
            _timerText.gameObject.SetActive(false);
        }
        else
        {
            _timerText.gameObject.SetActive(true);
            
            // Il countdown scorre a scendere
            float remainingTime = scenario.GetRemainingSlaTime();
            var (_, reactorTemp) = scenario.GetRuntimeDebugValues();
            
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            
            // Testo più tecnico per il timer
            _timerText.text = $"Tempo Residuo: {minutes:00}:{seconds:00}\nTemp Reattore: {reactorTemp:F1}°C";
            
            // ==========================================
            // MAGIA DEI COLORI: Il timer ora prende ESATTAMENTE 
            // lo stesso colore che ha il testo principale dell'HUD!
            // ==========================================
            _timerText.color = _instructionText.color;

            // SALVAVITA VR: Se mancano meno di 20 secondi o la temperatura è critica,
            // forziamo comunque il timer al ROSSO LAMPEGGIO per far mettere ansia al giocatore.
            if (remainingTime < 20f || reactorTemp > 88f)
            {
                _timerText.color = new Color(1f, 0.2f, 0.2f, 1f); // Rosso allarme
            }
        }
    }

    #region Public Event Handlers

    public void ShowEmergencyInstruction()
    {
        if (_instructionText == null) return;
        _instructionText.text = "ALLARME CRITICO:\nSovraccarico rilevato. Isolare l'alimentazione robot dal pannello SCADA.";
        _instructionText.color = new Color(1f, 0.35f, 0f, 1f);
        _instructionText.gameObject.SetActive(true);
    }

    public void ShowTankInstruction()
    {
        if (_instructionText == null) return;
        _instructionText.text = "ROBOT ISOLATO:\nRaggiungere il Reattore R-4 per avviare il raffreddamento d'emergenza.";
        _instructionText.color = new Color(1f, 0.8f, 0f, 1f);
        _instructionText.gameObject.SetActive(true);
    }

    public void ShowSuccessInstruction()
    {
        if (_instructionText == null) return;
        _instructionText.text = "RAFFREDDAMENTO ATTIVO:\nEvacuare immediatamente verso l'uscita di sicurezza.";
        _instructionText.color = new Color(0.2f, 1f, 0.8f, 1f); // Ciano tecnico
        _instructionText.gameObject.SetActive(true);
    }

    public void ShowFailureInstruction()
    {
        if (_instructionText == null) return;
        string motivo = ScenarioManager.Instance.LastFailureReason;
        _instructionText.text = $"<size=120%>SCENARIO FALLITO:</size>\n{motivo}";
        _instructionText.color = new Color(1f, 0.2f, 0.2f, 1f);
        _instructionText.gameObject.SetActive(true);
        if (_timerText != null) _timerText.color = new Color(1f, 0.2f, 0.2f, 1f);
    }

    public void ShowVictoryInstruction()
    {
        if (_instructionText == null) return;
        // Emoji rimossa per compatibilità font
        _instructionText.text = "<size=110%>EVACUAZIONE COMPLETATA:</size>\nSimulazione superata con successo!";
        _instructionText.color = new Color(0.2f, 1f, 0.4f, 1f);
        _instructionText.gameObject.SetActive(true);
        if (_timerText != null) _timerText.gameObject.SetActive(false);
    }

    public void ClearInstruction()
    {
        if (_instructionText == null) return;
        _instructionText.text = string.Empty;
        _instructionText.gameObject.SetActive(false);
    }
    #endregion
}