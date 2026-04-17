using UnityEngine;

/// <summary>
/// Cambia la texture di un materiale a runtime. 
/// Pensato per essere chiamato tramite UnityEvent.
/// </summary>
public class PanelTextureChanger : MonoBehaviour
{
    [Header("Riferimenti")]
    [Tooltip("Il Renderer dell'oggetto 3D (es. il pannello) che contiene il materiale.")]
    [SerializeField] private Renderer panelRenderer;

    [Tooltip("L'immagine da applicare quando il robot viene isolato.")]
    [SerializeField] private Texture2D isolatedTexture;

    [Header("Configurazione")]
    [Tooltip("Il nome della proprietà della texture nello shader. Per URP Lit è _BaseMap.")]
    [SerializeField] private string texturePropertyName = "_BaseMap";

    /// <summary>
    /// Metodo pubblico da collegare all'evento OnRobotIsolated dello ScenarioManager.
    /// </summary>
    public void ChangeToIsolatedTexture()
    {
        if (panelRenderer != null && isolatedTexture != null)
        {
            // Nota: accedere a .material crea un'istanza locale del materiale
            // per questo specifico oggetto, evitando di modificare l'asset originale.
            panelRenderer.material.SetTexture(texturePropertyName, isolatedTexture);
            Debug.Log("[PanelTextureChanger] Immagine del pannello aggiornata con successo.");
        }
        else
        {
            Debug.LogWarning("[PanelTextureChanger] Impossibile cambiare la texture: manca il Renderer o la Texture2D!");
        }
    }
}