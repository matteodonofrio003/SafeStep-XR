using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleportButton : MonoBehaviour
{
    [Header("Destinazione")]
    public TeleportDestination destination;

    [Header("UI (opzionale - auto-fill)")]
    public TMP_Text labelText;
    public Image iconImage;

    void Start()
    {
        // Popola automaticamente label e icona dalla destinazione
        if (destination != null)
        {
            if (labelText != null) labelText.text = destination.locationName;
            if (iconImage != null && destination.icon != null)
                iconImage.sprite = destination.icon;
        }

        // Collega il click
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (TeleportManager.Instance != null && destination != null)
            TeleportManager.Instance.TeleportTo(destination);
    }
}