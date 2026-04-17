using UnityEngine;
using TMPro;
using System.Collections;

public class InstructionHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _instructionText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("VR Smoothing")]
    [SerializeField, Range(1f, 15f)] private float _rotationSmoothSpeed = 8f;

    private Camera _vrCamera;
    private Coroutine _flashCoroutine;

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

            float remainingTime = scenario.GetRemainingSlaTime();
            var (_, reactorTemp) = scenario.GetRuntimeDebugValues();

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            _timerText.text = $"Tempo Residuo: {minutes:00}:{seconds:00}\nTemp Reattore: {reactorTemp:F1}°C";
            _timerText.color = _instructionText.color;

            if (remainingTime < 20f || reactorTemp > 88f)
            {
                _timerText.color = new Color(1f, 0.2f, 0.2f, 1f);
            }
        }
    }


    // =========================================================================
    // COUNTDOWN INIZIALE
    // =========================================================================

    /// <summary>
    /// Collega a ScenarioManager.OnCountdownTick (UnityEvent<int>).
    /// Mostra il conto alla rovescia prima del CascadeFailure.
    /// </summary>
    private Coroutine _countdownCoroutine;

    // Chiama questo metodo UNA SOLA VOLTA per avviare il countdown
    public void StartCountdown()
    {
        if (_instructionText == null) return;

        // Se c'è già un countdown attivo, lo fermiamo per sicurezza
        if (_countdownCoroutine != null) StopCoroutine(_countdownCoroutine);

        _countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        _instructionText.gameObject.SetActive(true);
        _instructionText.color = new Color(1f, 1f, 1f, 1f);

        // Ecco il tuo semplice ciclo for
        for (int i = 5; i > 0; i--)
        {
            _instructionText.text = $"Simulazione in avvio ....";

            // Flash di scala ad ogni tick
            if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
            _flashCoroutine = StartCoroutine(FlashScale(_instructionText.transform));

            // Aspetta esattamente 1 secondo prima di passare al numero successivo
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Collega a ScenarioManager.OnEmergencyTriggered.
    /// Rimuove il countdown quando la simulazione parte.
    /// </summary>
    public void HideCountdown()
    {
        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
        _instructionText.transform.localScale = Vector3.one;

        // Il testo verrà subito sovrascritto da ShowEmergencyInstruction(),
        // ma puliamo comunque per sicurezza
        _instructionText.text = string.Empty;
        _instructionText.gameObject.SetActive(false);
    }

    private IEnumerator FlashScale(Transform target)
    {
        target.localScale = Vector3.one * 1.4f;

        float elapsed = 0f;
        const float duration = 0.25f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.Lerp(Vector3.one * 1.4f, Vector3.one, elapsed / duration);
            yield return null;
        }

        target.localScale = Vector3.one;
    }


    // =========================================================================
    // PUBLIC EVENT HANDLERS
    // =========================================================================

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
        _instructionText.color = new Color(0.2f, 1f, 0.8f, 1f);
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
}