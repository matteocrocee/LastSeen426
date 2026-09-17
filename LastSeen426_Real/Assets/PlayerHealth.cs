using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("VITA")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("HUD")]
    public UIDocument hudDocument;

    private Label healthLabel;
    private VisualElement deathScreen;
    private Button restartButton;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (hudDocument != null)
        {
            VisualElement root =
                hudDocument.rootVisualElement;

            healthLabel =
                root.Q<Label>("HealthLabel");

            deathScreen =
                root.Q<VisualElement>("DeathScreen");

            restartButton =
                root.Q<Button>("RestartButton");

            if (restartButton != null)
            {
                restartButton.clicked += RestartGame;
            }
        }

        if (deathScreen != null)
        {
            deathScreen.style.display =
                DisplayStyle.None;
        }

        UpdateHealthHUD();
    }

    private void Update()
    {
        if (isDead &&
            Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0f,
                maxHealth
            );

        UpdateHealthHUD();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthHUD()
    {
        if (healthLabel != null)
        {
            healthLabel.text =
                "♥ VITA: " +
                Mathf.CeilToInt(currentHealth);
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("IL PLAYER È MORTO!");

        if (deathScreen != null)
        {
            deathScreen.style.display =
                DisplayStyle.Flex;
        }

        FirstPersonController controller =
            GetComponent<FirstPersonController>();

        if (controller != null)
        {
            controller.enabled = false;
        }

        UnityEngine.Cursor.lockState =
            UnityEngine.CursorLockMode.None;

        UnityEngine.Cursor.visible = true;
    }

    private void RestartGame()
    {
        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }
}