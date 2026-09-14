using UnityEngine;
using UnityEngine.UIElements;

public class DoorInteraction : MonoBehaviour
{
    [Header("INTERAZIONE")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Camera playerCamera;

    [Header("HUD")]
    [SerializeField] private UIDocument hudDocument;

    private DoorScript.Door currentDoor;
    private Label interactionLabel;

    private void Start()
    {
        // Trova la camera
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (playerCamera == null)
        {
            Debug.LogError("DoorInteraction: Main Camera non trovata!");
        }

        // Trova la scritta dell'HUD
        if (hudDocument != null)
        {
            interactionLabel =
                hudDocument.rootVisualElement.Q<Label>("InteractionLabel");
        }

        if (interactionLabel == null)
        {
            Debug.LogError(
                "DoorInteraction: InteractionLabel non trovato nell'HUD!"
            );
        }

        // All'inizio la scritta è nascosta
        HideInteractionText();
    }

    private void Update()
    {
        CheckInteraction();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void CheckInteraction()
    {
        currentDoor = null;

        if (playerCamera == null)
        {
            HideInteractionText();
            return;
        }

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance
        ))
        {
            DoorScript.Door door =
                hit.collider.GetComponentInParent<DoorScript.Door>();

            if (door != null && door.canOpen)
            {
                currentDoor = door;

                // Cambia la scritta in base allo stato della porta
                if (door.open)
                {
                    ShowInteractionText("[E] CHIUDI");
                }
                else
                {
                    ShowInteractionText("[E] APRI");
                }

                return;
            }
        }

        // Non stiamo guardando una porta utilizzabile
        HideInteractionText();
    }

    private void Interact()
    {
        if (currentDoor != null)
        {
            currentDoor.OpenDoor();
        }
    }

    private void ShowInteractionText(string text)
    {
        if (interactionLabel != null)
        {
            interactionLabel.text = text;
            interactionLabel.style.display = DisplayStyle.Flex;
        }
    }

    private void HideInteractionText()
    {
        if (interactionLabel != null)
        {
            interactionLabel.style.display = DisplayStyle.None;
        }
    }
}