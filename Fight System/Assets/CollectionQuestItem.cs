using UnityEngine;

public class CollectionQuestItem : MonoBehaviour
{
    [SerializeField] private Quest quest;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && quest.collectionQuest.active)
        {
            quest.collectionQuest.currentItems++;
            quest.collectionQuest.CountItems();
            Destroy(this.gameObject);
            quest.collectionQuest.ArrowDirection();
        }
    }
}
