using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    // Usiamo Awake invece di Start. Awake viene eseguito PRIMA che il gioco inizi a renderizzare le immagini.
    void Awake()
    {
        SpawnPlayerIstantaneo();
    }

    void SpawnPlayerIstantaneo()
    {
        GameObject xrOrigin = null;

        // Cerchiamo l'XR Origin anche se disattivato
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            if (obj.name == "XR Origin (XR Rig)" || obj.name == "XR Origin")
            {
                xrOrigin = obj;
                break;
            }
        }

        string spawnName = PlayerPrefs.GetString("SpawnPoint", "");

        // Se non c'è punto di spawn, accendi e basta
        if (string.IsNullOrEmpty(spawnName))
        {
            if (xrOrigin != null) xrOrigin.SetActive(true);
            return; // Usiamo return invece di yield break perché non è più una coroutine
        }

        TeleportDestination[] allPoints = FindObjectsByType<TeleportDestination>(FindObjectsSortMode.None);

        foreach (TeleportDestination point in allPoints)
        {
            if (point.locationName == spawnName)
            {
                if (xrOrigin != null)
                {
                    // Disabilita fisica
                    CharacterController cc = xrOrigin.GetComponent<CharacterController>();
                    if (cc != null) cc.enabled = false;

                    // Posizionamento
                    Camera xrCamera = xrOrigin.GetComponentInChildren<Camera>(true);
                    if (xrCamera != null)
                    {
                        Vector3 cameraOffset = xrCamera.transform.position - xrOrigin.transform.position;
                        cameraOffset.y = 0;
                        xrOrigin.transform.position = point.GetPosition() - cameraOffset;
                    }
                    else
                    {
                        xrOrigin.transform.position = point.GetPosition();
                    }

                    xrOrigin.transform.rotation = point.GetRotation();

                    // RIATTIVA TUTTO ISTANTANEAMENTE
                    xrOrigin.SetActive(true);
                    if (cc != null) cc.enabled = true;
                }
                break;
            }
        }

        PlayerPrefs.DeleteKey("SpawnPoint");
    }
}