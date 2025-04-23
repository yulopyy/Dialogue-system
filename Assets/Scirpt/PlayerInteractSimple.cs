using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager; // อ้างอิง DialogueManager
    public GameObject dialogueFrame; // Frame ของ Dialogue
    public float interactDistance = 5f; // ระยะกด E

    private GameObject currentNPC;
    private GameObject interactHint;
    private bool isInteracting = false;

    void Start()
    {
        dialogueFrame.SetActive(false); // ซ่อน Dialogue ตอนเริ่ม
    }

    void Update()
    {
        if (currentNPC != null)
        {
            float distance = Vector2.Distance(transform.position, currentNPC.transform.position);

            if (distance <= interactDistance)
            {
                if (interactHint != null && !interactHint.activeSelf)
                {
                    interactHint.SetActive(true); // เข้าใกล้ เปิด Hint
                }

                if (Input.GetKeyDown(KeyCode.E) && !isInteracting)
                {
                    dialogueFrame.SetActive(true); // แสดง Frame
                    dialogueManager.StartDialogue(); // สั่งเริ่ม Dialogue
                    isInteracting = true;

                    if (interactHint != null)
                    {
                        interactHint.SetActive(false); // กดแล้วซ่อน Hint
                    }
                }
            }
            else
            {
                if (interactHint != null && interactHint.activeSelf)
                {
                    interactHint.SetActive(false); // ออกระยะ ซ่อน Hint
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            currentNPC = collision.gameObject;

            // หาลูกชื่อ InteractHint แบบปลอดภัย
            Transform hintTransform = currentNPC.transform.Find("InteractHint");
            if (hintTransform != null)
            {
                interactHint = hintTransform.gameObject;
                interactHint.SetActive(false); // กันพัง เผื่อเผลอเปิดค้าง
            }
            else
            {
                Debug.LogWarning("InteractHint not found under NPC: " + currentNPC.name);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            if (interactHint != null)
            {
                interactHint.SetActive(false); // ออกนอกระยะ ปิด Hint
            }
            currentNPC = null;
            interactHint = null;
            isInteracting = false;
        }
    }
}
