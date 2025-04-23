using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactHint; // ข้อความ "E to Interact"
    private bool isPlayerInRange = false;
    private DialogueManager dialogueManager;

    void Start()
    {
        if (interactHint != null)
            interactHint.SetActive(false);

        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Interacting with NPC...");
            dialogueManager.StartDialogue(); // เริ่มต้นบทสนทนา
            if (interactHint != null)
                interactHint.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactHint != null)
                interactHint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactHint != null)
                interactHint.SetActive(false);
        }
    }
}
