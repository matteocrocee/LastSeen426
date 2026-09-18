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
    private PickupItem currentItem;

    private Label interactionLabel;

    private Inventory inventory;

    private void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (playerCamera == null)
        {
            Debug.LogError(
                "DoorInteraction: Main Camera non trovata!"
            );
        }

        if (hudDocument != null)
        {
            interactionLabel =
                hudDocument.rootVisualElement
                .Q<Label>("InteractionLabel");
        }

        if (interactionLabel == null)
        {
            Debug.LogError(
                "DoorInteraction: InteractionLabel non trovato nell'HUD!"
            );
        }

        inventory =
            GetComponent<Inventory>();

        if (inventory == null)
        {
            inventory =
                gameObject.AddComponent<Inventory>();
        }

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
        currentItem = null;

        if (playerCamera == null)
        {
            HideInteractionText();
            return;
        }

        Ray ray =
            new Ray(
                playerCamera.transform.position,
                playerCamera.transform.forward
            );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance
        ))
        {
            PickupItem item =
                hit.collider
                .GetComponentInParent<PickupItem>();

            if (item != null)
            {
                currentItem = item;

                ShowInteractionText(
                    "[E] RACCOGLI"
                );

                return;
            }

            DoorScript.Door door =
                hit.collider
                .GetComponentInParent<DoorScript.Door>();

            if (door != null &&
                door.canOpen)
            {
                currentDoor = door;

                if (door.open)
                {
                    ShowInteractionText(
                        "[E] CHIUDI"
                    );

                    return;
                }

                if (door.RequiresKey())
                {
                    if (inventory != null &&
                        inventory.HasItem(
                            door.GetRequiredKey()
                        ))
                    {
                        ShowInteractionText(
                            "[E] USA LA CHIAVE"
                        );
                    }
                    else
                    {
                        ShowInteractionText(
                            "[E] SERVE UNA CHIAVE"
                        );
                    }

                    return;
                }

                ShowInteractionText(
                    "[E] APRI"
                );

                return;
            }
        }

        HideInteractionText();
    }

    private void Interact()
    {
        if (currentItem != null)
        {
            currentItem.PickUp(
                inventory
            );

            return;
        }

        if (currentDoor != null)
        {
            currentDoor.OpenDoor(
                inventory
            );
        }
    }

    private void ShowInteractionText(
        string text
    )
    {
        if (interactionLabel != null)
        {
            interactionLabel.text = text;

            interactionLabel.style.display =
                DisplayStyle.Flex;
        }
    }

    private void HideInteractionText()
    {
        if (interactionLabel != null)
        {
            interactionLabel.style.display =
                DisplayStyle.None;
        }
    }
}