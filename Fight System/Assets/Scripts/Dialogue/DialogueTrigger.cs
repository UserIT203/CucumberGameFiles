using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Message[] messages;
    public AfterQuest[] messagesAfterQuest;
    public Actor[] actors;

    public bool isTalking;

    public void StartDialogue()
    {
        if (this.GetComponent<Enemy>() != null)
        {
            FindObjectOfType<DialogueManager>().enemyAttacked = this.GetComponent<Enemy>();
            FindObjectOfType<DialogueManager>().npcQuest = null;
        }

        if (this.GetComponent<Quest>() != null)
        {
            FindObjectOfType<DialogueManager>().npcQuest = this.GetComponent<Quest>();
            FindObjectOfType<DialogueManager>().enemyAttacked = null;
        }

        FindObjectOfType<DialogueManager>().OpenDialogue(messages, actors, messagesAfterQuest);
    }
}

[System.Serializable]
public class Message
{
    public int actorID;
    public string message;
    public int emotionID;
}

[System.Serializable]
public class AfterQuest
{
    public int actorID;
    public string message;
    public int emotionID;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite[] emotionList;
}
