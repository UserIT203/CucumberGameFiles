using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTypes : MonoBehaviour
{
    public CollectionQuest collectionQuest;

    public void StartQuest()
    {
        collectionQuest.StartCollectQuest();
    }
    
}




