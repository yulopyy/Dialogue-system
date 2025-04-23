using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueSet
    {
        public string[] sentences;
        public int[] characterIndexes;   // Sprite Index สำหรับแต่ละประโยค
        public string[] requiredTags;    // ไอเทมที่ต้องเก็บให้ครบก่อนข้าม Set
    }

    public Text dialogueText;
    public Text continueHint;
    public Image characterImage;
    public Sprite[] characterSprites;
    public CanvasGroup dialogueCanvasGroup;

    public DialogueSet[] dialogueSets;  // == Timeline Dialogues ทั้งหมด

    public float typingSpeed = 0.05f;

    private int currentSetIndex = 0;
    private int currentSentenceIndex = 0;
    private bool isTyping = false;

    private HashSet<string> collectedTags = new HashSet<string>();  // เก็บแท็กที่เก็บได้แล้ว

    void Start()
    {
        continueHint.gameObject.SetActive(false);
        dialogueText.text = "";

        if (dialogueCanvasGroup != null)
        {
            dialogueCanvasGroup.alpha = 0f;
            dialogueCanvasGroup.interactable = false;
            dialogueCanvasGroup.blocksRaycasts = false;
        }
    }

    public void StartDialogue()
    {
        if (dialogueSets.Length == 0) return;

        currentSentenceIndex = 0;

        if (dialogueCanvasGroup != null)
        {
            dialogueCanvasGroup.alpha = 1f;
            dialogueCanvasGroup.interactable = true;
            dialogueCanvasGroup.blocksRaycasts = true;
        }

        dialogueText.text = "";
        StartCoroutine(TypeSentence(dialogueSets[currentSetIndex].sentences[currentSentenceIndex]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueSets[currentSetIndex].sentences[currentSentenceIndex];
                isTyping = false;
                continueHint.gameObject.SetActive(true);
            }
            else
            {
                continueHint.gameObject.SetActive(false);
                NextSentence();
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        int[] charIndexes = dialogueSets[currentSetIndex].characterIndexes;

        if (charIndexes != null && currentSentenceIndex < charIndexes.Length && charIndexes[currentSentenceIndex] < characterSprites.Length)
        {
            characterImage.sprite = characterSprites[charIndexes[currentSentenceIndex]];
        }
        else
        {
            Debug.LogWarning($"Character sprite not found for index {currentSentenceIndex}!");
            if (characterSprites.Length > 0)
                characterImage.sprite = characterSprites[0];
        }

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueHint.gameObject.SetActive(true);
    }

    void NextSentence()
    {
        if (currentSentenceIndex < dialogueSets[currentSetIndex].sentences.Length - 1)
        {
            currentSentenceIndex++;
            StartCoroutine(TypeSentence(dialogueSets[currentSetIndex].sentences[currentSentenceIndex]));
        }
        else
        {
            Debug.Log($"End of Dialogue Set {currentSetIndex + 1}");

            dialogueText.text = "";

            if (dialogueCanvasGroup != null)
            {
                dialogueCanvasGroup.alpha = 0f;
                dialogueCanvasGroup.interactable = false;
                dialogueCanvasGroup.blocksRaycasts = false;
            }

            CheckIfReadyForNextSet();
        }
    }

    public void NotifyItemCollected(string tag)
    {
        collectedTags.Add(tag);
        Debug.Log($"Collected item with tag: {tag}");

        CheckIfReadyForNextSet();
    }

    void CheckIfReadyForNextSet()
    {
        if (currentSetIndex >= dialogueSets.Length - 1)
            return; // ไม่มีเซ็ตถัดไปแล้ว

        string[] requiredTags = dialogueSets[currentSetIndex].requiredTags;

        if (requiredTags != null && requiredTags.Length > 0)
        {
            foreach (var requiredTag in requiredTags)
            {
                if (!collectedTags.Contains(requiredTag))
                {
                    // ยังเก็บไม่ครบ
                    return;
                }
            }
        }

        // ถ้าเก็บครบแล้ว → ข้ามไป Set ถัดไป
        currentSetIndex++;
        collectedTags.Clear(); // ล้างแท็กที่เก็บได้ เพราะเซ็ตใหม่อาจมีแท็กใหม่
        Debug.Log($"Ready for Dialogue Set {currentSetIndex + 1}");
    }
}
