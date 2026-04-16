using UnityEngine;

public class ExitSensor : MonoBehaviour
{
    public EmergencyManager manager; // Trascineremo qui il nostro manager dall'Inspector

    // Questa funzione magica scatta DA SOLA quando qualcosa entra in un "Trigger"
    private void OnTriggerEnter(Collider other)
    {
        // Controlla se chi è entrato è il giocatore (XR Origin)
        // (Assicurati che il tuo XR Origin abbia il Tag impostato su "Player" in alto a sinistra nell'Inspector)
        if (other.CompareTag("Player"))
        {
            manager.ReachExit();
        }
    }
}