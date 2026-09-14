using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    [Header("VITA")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("HUD")]
    public UIDocument hudDocument;

    private Label healthLabel;
    private VisualElement deathScreen;

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
        }

        if (deathScreen != null)
        {
            deathScreen.style.display =
                DisplayStyle.None;
        }

        UpdateHealthHUD();
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
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
    }
}