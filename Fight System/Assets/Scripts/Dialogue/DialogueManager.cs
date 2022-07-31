using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image playerImage;
    public Image NPCImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    Message[] currentMessages;
    AfterQuest[] currentMessagesAfterQuest;
    Actor[] currentActors;
    private int activeMessage;
    public static bool isActive = false;

    public Enemy enemyAttacked;
    public Quest npcQuest;

    private void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;
    }

    public void OpenDialogue(Message[] messages, Actor[] actors, AfterQuest[] messageAfterQuest)
    {
        currentMessages = messages;
        currentMessagesAfterQuest = messageAfterQuest;
        currentActors = actors;
        activeMessage = 0;

        DisplayMessage();

        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();

        isActive = true;
    }

    private void DisplayMessage()
    {
        dynamic messageToDisplay;
        if (npcQuest == null || !npcQuest.isEnd)
            messageToDisplay = currentMessages[activeMessage];
        else
            messageToDisplay = currentMessagesAfterQuest[activeMessage];

        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.name;
        if (actorName.text == "CUCUMBER")
        {
            NPCImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);

            playerImage.color = Color.white;
            playerImage.sprite = actorToDisplay.emotionList[messageToDisplay.emotionID];
        }
        else
        {
            playerImage.color = new Color(0.5f, 0.5f, 0.5f, 1f);

            NPCImage.color = Color.white;
            NPCImage.sprite = actorToDisplay.emotionList[messageToDisplay.emotionID];
        }

        AnimateTextColor();
    }

    public void NextMessage()
    {
        activeMessage++;

        int currentNumber = 0;
        if (npcQuest == null || !npcQuest.isEnd)
        {
            currentNumber = currentMessages.Length;
        }
        else
        {
            currentNumber = currentMessagesAfterQuest.Length;
        }
        

        if (activeMessage < currentNumber)
        {
            DisplayMessage();
        }
        else //конец диалога
        {
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();//закрытие окна диалога

            if (enemyAttacked != null)
                enemyAttacked.Fight();

            if (npcQuest != null)
                npcQuest.StartQuest();

        }
    }

    private void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && isActive)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && isActive)
                NextMessage();
        }
    }
}

