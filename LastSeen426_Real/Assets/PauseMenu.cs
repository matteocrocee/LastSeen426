using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("HUD")]
    public UIDocument hudDocument;

    private VisualElement pauseScreen;

    private Button resumeButton;
    private Button restartButton;
    private Button quitButton;

    private FirstPersonController firstPersonController;

    private bool isPaused = false;

    private void Start()
    {
        // Trova il controller del Player
        firstPersonController =
            GetComponent<FirstPersonController>();

        if (hudDocument == null)
        {
            Debug.LogError(
                "PauseMenu: manca il riferimento al HUD!"
            );

            return;
        }

        VisualElement root =
            hudDocument.rootVisualElement;

        pauseScreen =
            root.Q<VisualElement>("PauseScreen");

        resumeButton =
            root.Q<Button>("ResumeButton");

        restartButton =
            root.Q<Button>("PauseRestartButton");

        quitButton =
            root.Q<Button>("QuitButton");

        // Collegamento pulsante RIPRENDI
        if (resumeButton != null)
        {
            resumeButton.clicked += ResumeGame;
        }
        else
        {
            Debug.LogError(
                "PauseMenu: ResumeButton non trovato!"
            );
        }

        // Collegamento pulsante RICOMINCIA
        if (restartButton != null)
        {
            restartButton.clicked += RestartLevel;
        }
        else
        {
            Debug.LogError(
                "PauseMenu: PauseRestartButton non trovato!"
            );
        }

        // Collegamento pulsante ESCI
        if (quitButton != null)
        {
            quitButton.clicked += QuitGame;
        }
        else
        {
            Debug.LogError(
                "PauseMenu: QuitButton non trovato!"
            );
        }

        ClosePauseMenu();

        // Stato iniziale corretto
        Time.timeScale = 1f;

        UnityEngine.Cursor.lockState =
            UnityEngine.CursorLockMode.Locked;

        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // =====================================================
    // PAUSA
    // =====================================================

    private void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0f;

        // Blocca movimento + telecamera
        if (firstPersonController != null)
        {
            firstPersonController.enabled = false;
        }

        if (pauseScreen != null)
        {
            pauseScreen.style.display =
                DisplayStyle.Flex;
        }

        // Mostra il mouse
        UnityEngine.Cursor.lockState =
            UnityEngine.CursorLockMode.None;

        UnityEngine.Cursor.visible = true;

        Debug.Log("GIOCO IN PAUSA");
    }

    // =====================================================
    // RIPRENDI
    // =====================================================

    private void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;

        ClosePauseMenu();

        // Riattiva movimento + telecamera
        if (firstPersonController != null)
        {
            firstPersonController.enabled = true;
        }

        // Blocca nuovamente il mouse
        UnityEngine.Cursor.lockState =
            UnityEngine.CursorLockMode.Locked;

        UnityEngine.Cursor.visible = false;

        Debug.Log("GIOCO RIPRESO");
    }

    // =====================================================
    // CHIUDI MENU
    // =====================================================

    private void ClosePauseMenu()
    {
        if (pauseScreen != null)
        {
            pauseScreen.style.display =
                DisplayStyle.None;
        }
    }

    // =====================================================
    // RICOMINCIA LIVELLO
    // =====================================================

    private void RestartLevel()
    {
        Debug.Log("RIAVVIO DEL LIVELLO...");

        // Riporta il tempo alla normalità
        Time.timeScale = 1f;

        // Evita che il mouse rimanga libero
        UnityEngine.Cursor.lockState =
            UnityEngine.CursorLockMode.Locked;

        UnityEngine.Cursor.visible = false;

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.name
        );
    }

    // =====================================================
    // ESCI DAL GIOCO
    // =====================================================

    private void QuitGame()
    {
        Debug.Log("USCITA DAL GIOCO...");

        Time.timeScale = 1f;

        Application.Quit();
    }

    // =====================================================
    // SICUREZZA
    // =====================================================

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}