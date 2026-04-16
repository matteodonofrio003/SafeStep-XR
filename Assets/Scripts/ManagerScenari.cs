using UnityEngine;
using TMPro; // FONDAMENTALE: aggiungi questa riga in cima!

public class ManagerScenari : MonoBehaviour
{
    [System.Serializable]
    public struct DatiScenario
    {
        public string nomeScenario;
        public Vector3 posizione;
        public Vector3 rotazione;
    }

    [Header("Riferimenti Obbligatori")]
    public GameObject player;
    public GameObject canvasUI;

    [Header("Didascalia")]
    public TextMeshProUGUI campoDidascalia; // <-- NUOVA RIGA per il testo

    [Header("Configurazione Coordinate")]
    public DatiScenario[] scenari;

    void Start()
    {
        Teleport(0);
        if (canvasUI != null) canvasUI.SetActive(true);

        // Assicuriamoci che all'avvio la didascalia sia spenta
        if (campoDidascalia != null) campoDidascalia.gameObject.SetActive(false);
    }

    public void SelezionaScenario(int index)
    {
        Teleport(index);
        // if(canvasUI != null) canvasUI.SetActive(false); 
    }

    private void Teleport(int index)
    {
        if (scenari == null || index >= scenari.Length || player == null) return;

        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        player.transform.position = scenari[index].posizione;
        player.transform.rotation = Quaternion.Euler(scenari[index].rotazione);

        if (controller != null) controller.enabled = true;
    }

    // --- NUOVE FUNZIONI PER LA DIDASCALIA ---
    public void MostraTestoPredefinito(string testoScelto)
    {
        if (campoDidascalia != null)
        {
            campoDidascalia.text = testoScelto;
            campoDidascalia.gameObject.SetActive(true);
        }
    }

    public void NascondiTesto()
    {
        if (campoDidascalia != null)
        {
            campoDidascalia.gameObject.SetActive(false);
        }
    }
}