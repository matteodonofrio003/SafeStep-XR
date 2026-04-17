using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TeleportButton : MonoBehaviour
{
    [Header("Destinazione stessa scena")]
    public TeleportDestination destination;

    [Header("Destinazione altra scena")]
    public string targetSceneName; // scrivi qui il nome della scena
    public string spawnPointName;

    [Header("UI (opzionale)")]
    public TMP_Text labelText;
    public Image iconImage;

    void Start()
    {
        if (destination != null)
        {
            if (labelText != null) labelText.text = destination.locationName;
            if (iconImage != null && destination.icon != null)
                iconImage.sprite = destination.icon;
        }

        Button btn = GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            // ✅ SALVA lo spawn point PRIMA di cambiare scena
            if (!string.IsNullOrEmpty(spawnPointName))
            {
                PlayerPrefs.SetString("SpawnPoint", spawnPointName);
                PlayerPrefs.Save(); // ← QUESTA RIGA MANCAVA!
            }

            SceneManager.LoadScene(targetSceneName);
            return;
        }

        if (TeleportManager.Instance != null && destination != null)
            TeleportManager.Instance.TeleportTo(destination);
    }

    void OnDestroy()
    {
        Button btn = GetComponent<Button>();
        if (btn != null) btn.onClick.RemoveListener(OnClick);
    }
}