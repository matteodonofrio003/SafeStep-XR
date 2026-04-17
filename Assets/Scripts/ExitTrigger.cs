using UnityEngine;

/// <summary>
/// Da assegnare al BoxCollider (Is Trigger = true) posto davanti all'uscita.
/// </summary>
public class ExitTrigger : MonoBehaviour
{
    [Tooltip("Trascina qui la porta (il GameObject) che ha lo script DoorOpener attaccato.")]
    [SerializeField] private DoorOpener doorToOpen;

    // Flag per assicurarci che l'uscita venga triggerata una volta sola
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Se è già stato attivato, ignoriamo altri ingressi
        if (hasTriggered) return;

        // Verifica se chi entra nel trigger è il giocatore (adatta il controllo al tuo rig VR)
        // Spesso in VR basta controllare il tag "Player" o la presenza della MainCamera/CharacterController
        if (other.CompareTag("Player") || other.GetComponentInChildren<Camera>() != null)
        {
            hasTriggered = true;

            // 1. Ferma il timer e segna la vittoria nello ScenarioManager
            ScenarioManager.Instance.ReachExit();

            // 2. Apri la porta
            if (doorToOpen != null)
            {
                doorToOpen.OpenDoor();
                Debug.Log("[ExitTrigger] Porta dell'uscita in apertura!");
            }
            else
            {
                Debug.LogWarning("[ExitTrigger] Attenzione: Nessun DoorOpener assegnato nell'Inspector!");
            }
        }
    }
}