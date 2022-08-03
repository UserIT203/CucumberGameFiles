using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static Dictionary<string, bool> questIsActive = new Dictionary<string, bool>();

    [SerializeField] private GameObject[] allQuestButton; 
    [SerializeField] private GameObject questMainObj;

    [HideInInspector] public GameObject currentQuest;
    private bool isActive = false;

    private int currentQuestObjID = 0;

    private void Awake()
    {
        for (int i = 0; i < allQuestButton.Length; i++)
        {
            string questObj_name = allQuestButton[i].name;
            questIsActive.Add(questObj_name, false); 
        }
    }

    public GameObject SetItemQuest()
    {
        GameObject itemQuest = allQuestButton[currentQuestObjID];
        currentQuestObjID++;

        return itemQuest;
    }

    private void SetTextOnQuestButton()
    {
        questMainObj.transform.GetChild(0).GetComponent<Text>().text =
            currentQuest.transform.GetChild(0).GetComponent<Text>().text;
    }

    public void ChangeQuestMainObj(GameObject _questObj)
    {
        currentQuest = _questObj;
        SetTextOnQuestButton();
    }

    //UI 
    [Header("UI Component")]
    [SerializeField] private GameObject questMenuPanel;
    public void OnClickGoToQuestMenu() => questMenuPanel.SetActive(true);
    public void OnClickExitToQuestMenu() => questMenuPanel.SetActive(false);


    private void Update()
    {
        if (!isActive)
        {
            foreach (GameObject quest in allQuestButton)
            {
                if (questIsActive[quest.name] == true)
                {
                    isActive = true;

                    currentQuest = quest;
                    SetTextOnQuestButton();

                    if (questMainObj.active == false)
                        questMainObj.SetActive(true);
                }
            }
        }else
            SetTextOnQuestButton();
    }
}
