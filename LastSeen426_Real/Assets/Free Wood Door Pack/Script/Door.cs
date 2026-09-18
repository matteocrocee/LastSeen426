using UnityEngine;
using UnityEngine.SceneManagement;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        [Header("PORTA")]
        public bool canOpen = true;
        public bool open = false;
        public float rotationSpeed = 5f;
        public float openAngle = 90f;

        [Header("CHIAVE")]
        public bool requiresKey = false;
        public string requiredKey = "Chiave";

        [Header("CAMBIO LIVELLO")]
        public bool changesLevel = false;
        public string nextSceneName = "SecondoLivello";
        public float levelLoadDelay = 1f;

        [Header("AUDIO")]
        public AudioSource asource;
        public AudioClip openDoor;
        public AudioClip closeDoor;

        private Quaternion closedRotation;
        private Quaternion openRotation;

        private bool levelLoading = false;

        private void Start()
        {
            asource = GetComponent<AudioSource>();

            closedRotation = transform.localRotation;

            openRotation =
                closedRotation *
                Quaternion.Euler(
                    0f,
                    openAngle,
                    0f
                );
        }

        private void Update()
        {
            Quaternion targetRotation;

            if (open)
            {
                targetRotation = openRotation;
            }
            else
            {
                targetRotation = closedRotation;
            }

            transform.localRotation =
                Quaternion.Slerp(
                    transform.localRotation,
                    targetRotation,
                    Time.deltaTime * rotationSpeed
                );
        }

        // =====================================================
        // METODO PER I VECCHI SCRIPT DEL DOOR PACK
        // =====================================================

        public void OpenDoor()
        {
            OpenDoor(null);
        }

        // =====================================================
        // METODO PRINCIPALE CON INVENTARIO
        // =====================================================

        public void OpenDoor(Inventory inventory)
        {
            if (!canOpen)
            {
                return;
            }

            // Se la porta è già aperta, la chiudiamo
            if (open)
            {
                CloseDoor();
                return;
            }

            // Controllo chiave
            if (requiresKey)
            {
                if (inventory == null)
                {
                    Debug.Log(
                        "Questa porta richiede la chiave: " +
                        requiredKey
                    );

                    return;
                }

                if (!inventory.HasItem(requiredKey))
                {
                    Debug.Log(
                        "Chiave mancante: " +
                        requiredKey
                    );

                    return;
                }

                // Consuma la chiave
                inventory.RemoveItem(requiredKey);

                Debug.Log(
                    "Chiave utilizzata: " +
                    requiredKey
                );
            }

            // Apertura porta
            open = true;

            if (asource != null &&
                openDoor != null)
            {
                asource.clip = openDoor;
                asource.Play();
            }

            // Cambio livello
            if (changesLevel &&
                !levelLoading)
            {
                levelLoading = true;

                Invoke(
                    nameof(LoadNextLevel),
                    levelLoadDelay
                );
            }
        }

        // =====================================================
        // CHIUSURA
        // =====================================================

        private void CloseDoor()
        {
            open = false;

            if (asource != null &&
                closeDoor != null)
            {
                asource.clip = closeDoor;
                asource.Play();
            }
        }

        // =====================================================
        // CAMBIO SCENA
        // =====================================================

        private void LoadNextLevel()
        {
            SceneManager.LoadScene(nextSceneName);
        }

        // =====================================================
        // FUNZIONI USATE DA DOORINTERACTION
        // =====================================================

        public bool RequiresKey()
        {
            return requiresKey;
        }

        public string GetRequiredKey()
        {
            return requiredKey;
        }

        public bool CanPlayerOpen(Inventory inventory)
        {
            if (!canOpen)
            {
                return false;
            }

            if (!requiresKey)
            {
                return true;
            }

            if (inventory == null)
            {
                return false;
            }

            return inventory.HasItem(requiredKey);
        }
    }
}