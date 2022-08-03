using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Quest : MonoBehaviour
{
    [HideInInspector] public Image questItem;

    public Color completedColor;
    public Color activeColor;
    public Color currentColor;
    public QuestArrow arrow;

    public Quest[] allQuest;

    public CollectionQuest collectionQuest;
    public bool isActive = false;
    public bool isEnd = false;

    private QuestManager questManager;

    private void Start()
    {
        questManager = GameObject.Find("QuestManger").GetComponent<QuestManager>();

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
        //arrow.gameObject.SetActive(true);
        //collectionQuest.ArrowDirection();
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
    [SerializeField] private QuestManager questManagerScripts;

    [SerializeField] private string descriptions;
    [SerializeField] private Quest questNPC;
    [SerializeField] private Transform[] itemList;

    [HideInInspector] public GameObject questObj;
    public bool active;

    public int currentItems;
    private int countItems;

    [HideInInspector] public string mainText;
    public void StartCollectQuest()
    {
        if (!active)
        {
            questObj = questManagerScripts.SetItemQuest();

            currentItems = 0;
            countItems = itemList.Length;

            active = true;

            questNPC.questItem = this.questObj.GetComponent<Image>();

            mainText = descriptions + " " + currentItems + " | " + countItems;
            questObj.transform.GetChild(0).GetComponent<Text>().text = mainText;

            questObj.GetComponent<Button>().onClick.AddListener(() => OnClickButtonQuest());
            questObj.SetActive(true);
            QuestManager.questIsActive[questObj.name] = true;
        }
    }

    public void OnClickButtonQuest()
    {
        questManagerScripts.ChangeQuestMainObj(questObj);
    }

    //public void ArrowDirection()
    //{
    //    arrow.target = itemList[currentItems];
    //}

    public void CountItems()
    {
        mainText = descriptions + " " + currentItems + " | " + countItems;
        questObj.transform.GetChild(0).GetComponent<Text>().text = mainText;

        if (currentItems == countItems)
        {
            questNPC.FinishQuest();
        }
    }

}