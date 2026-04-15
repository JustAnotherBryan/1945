using System.Diagnostics;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;

    private bool isOpen = false;
    private bool playerInRange = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UnityEngine.Debug.Log($"Press E to open {gameObject.name}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void Update()
    {
        // Check for input when player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }

        // Rotate the door
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        UnityEngine.Debug.Log($"Door {gameObject.name} is now {(isOpen ? "open" : "closed")}");
    }
}