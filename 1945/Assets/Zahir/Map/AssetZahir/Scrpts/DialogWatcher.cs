using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueWatcher : MonoBehaviour
{
    public GameObject restartButton;
    public GameObject dialogueObject; // Drag your Dialogue GameObject here

    [Header("Scene Management")]
    public string sceneName = ""; // Leave empty to reload current scene

    [Header("Mouse Settings")]
    public bool unlockMouseOnDialogueEnd = true;
    public CursorLockMode desiredLockMode = CursorLockMode.None; // None = unlocked, Locked = locked

    private bool dialogueWasActive = true;

    void Start()
    {
        // Hide restart button at start
        if (restartButton != null)
            restartButton.SetActive(false);
    }

    void Update()
    {
        // Check if dialogue exists but is disabled (meaning it finished)
        if (dialogueObject != null && !dialogueObject.activeInHierarchy && dialogueWasActive)
        {
            // Dialogue just finished!
            ShowRestartButton();
            UnlockMouse(); // Unlock mouse when dialogue ends
            dialogueWasActive = false;
        }

        // Update the tracking variable
        if (dialogueObject != null)
            dialogueWasActive = dialogueObject.activeInHierarchy;
    }

    void ShowRestartButton()
    {
        if (restartButton != null)
        {
            restartButton.SetActive(true);

            UnityEngine.UI.Button button = restartButton.GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(RestartGame);
            }
        }
    }

    void UnlockMouse()
    {
        if (unlockMouseOnDialogueEnd)
        {
            Cursor.lockState = desiredLockMode;
            Cursor.visible = (desiredLockMode == CursorLockMode.None);

            UnityEngine.Debug.Log("Mouse unlocked after dialogue!");
        }
    }

    public void RestartGame()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}