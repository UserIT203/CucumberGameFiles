using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Quest : MonoBehaviour
{
    public Image questItem;

    public Color completedColor;
    public Color activeColor;
    public Color currentColor;
    public QuestArrow arrow;

    public Quest[] allQuest;

    public CollectionQuest collectionQuest;
    public bool isActive = false;
    public bool isEnd = false;

    private void Start()
    {
        allQuest = FindObjectsOfType<Quest>();
        currentColor = questItem.color;
    }

    public void StartQuest()
    {
        isActive = true;
        collectionQuest.StartCollectQuest();
    }

    public void FinishQuest()
    {
        questItem.GetComponent<Button>().interactable = false;
        currentColor = completedColor;
        questItem.color = completedColor;
        arrow.gameObject.SetActive(false);
        isEnd = true;
    }

    public void OnQuestClick()
    {
        arrow.gameObject.SetActive(true);
        collectionQuest.ArrowDirection();
        foreach (Quest quest in allQuest)
        {
            quest.questItem.color = quest.currentColor;
        }

        questItem.color = activeColor;
    }

}

[System.Serializable]
public class CollectionQuest
{
    [SerializeField] private GameObject questObj;

    [SerializeField] private string descriptions;
    [SerializeField] private int countItems;
    [SerializeField] private Quest questManager;
    [SerializeField] private Transform[] itemList;
    public bool active;

    public QuestArrow arrow;

    [SerializeField]
    public int currentItems = 0;

    public void StartCollectQuest()
    {
        active = true;

        questManager.questItem = this.questObj.GetComponent<Image>();

        questObj.transform.GetChild(0).GetComponent<Text>().text = descriptions + "\n" + currentItems + " | " + countItems;

        questObj.SetActive(true);
    }

    public void ArrowDirection()
    {
        arrow.target = itemList[currentItems];
    }

    public void CountItems()
    {
        Debug.Log(currentItems);

        questObj.transform.GetChild(0).GetComponent<Text>().text = descriptions + "\n" + currentItems + " | " + countItems;

        if (currentItems == countItems)
        {
            questManager.FinishQuest();
        }
    }

}