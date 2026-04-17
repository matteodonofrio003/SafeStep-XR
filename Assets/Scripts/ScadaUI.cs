using UnityEngine;
using TMPro;

public class ScadaUI : MonoBehaviour
{
    [Header("Riferimenti Testo")]
    public TextMeshProUGUI pressureTextUI;
    public TextMeshProUGUI tempTextUI;

    /// <summary>
    /// Metodo da collegare a ReactorManager.OnTelemetryUpdated
    /// </summary>
    public void UpdateTelemetry(float pressure, float temp)
    {
        if (pressureTextUI != null) 
            pressureTextUI.text = "PRESSIONE: " + pressure.ToString("F1") + " BAR";
            
        if (tempTextUI != null) 
            tempTextUI.text = "TEMP: " + temp.ToString("F1") + " °C";
            
        // Opzionale: cambia colore in rosso se la pressione supera i 4.5
        if (pressureTextUI != null)
        {
             if(pressure >= 4.5f) pressureTextUI.color = Color.red;
             else pressureTextUI.color = Color.white;
        }
    }
}