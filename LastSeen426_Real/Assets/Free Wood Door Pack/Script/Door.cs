using UnityEngine;

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

        [Header("AUDIO")]
        public AudioSource asource;
        public AudioClip openDoor;
        public AudioClip closeDoor;

        private Quaternion closedRotation;
        private Quaternion openRotation;

        private void Start()
        {
            asource = GetComponent<AudioSource>();

            closedRotation = transform.localRotation;

            openRotation = closedRotation * Quaternion.Euler(
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

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        public void OpenDoor()
        {
            // Se la porta non è apribile, non fare niente
            if (!canOpen)
            {
                return;
            }

            open = !open;

            if (asource != null)
            {
                if (open && openDoor != null)
                {
                    asource.clip = openDoor;
                    asource.Play();
                }
                else if (!open && closeDoor != null)
                {
                    asource.clip = closeDoor;
                    asource.Play();
                }
            }
        }
    }
}