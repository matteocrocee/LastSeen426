using UnityEngine;

public class Drawer : MonoBehaviour
{
    [Header("CASSETTO")]
    [SerializeField] private string drawerName = "drawer1_cabinet2";

    [Header("APERTURA")]
    [SerializeField] private float openDistance = 0.5f;
    [SerializeField] private float openSpeed = 3f;

    private Transform drawer;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;

    private void Start()
    {
        Transform[] children =
            GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == drawerName)
            {
                drawer = child;
                break;
            }
        }

        if (drawer == null)
        {
            Debug.LogError(
                "Drawer: non trovo il cassetto '" +
                drawerName +
                "' dentro " +
                gameObject.name
            );

            enabled = false;
            return;
        }

        closedPosition = drawer.localPosition;

        openPosition =
            closedPosition +
            Vector3.forward * openDistance;

        // TEST TEMPORANEO
        OpenDrawer();
    }

    private void Update()
    {
        if (drawer == null)
            return;

        Vector3 targetPosition =
            isOpen
                ? openPosition
                : closedPosition;

        drawer.localPosition =
            Vector3.Lerp(
                drawer.localPosition,
                targetPosition,
                Time.deltaTime * openSpeed
            );
    }

    public void OpenDrawer()
    {
        isOpen = true;
    }

    public void CloseDrawer()
    {
        isOpen = false;
    }
}