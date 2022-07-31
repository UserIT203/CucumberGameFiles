using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Collisions Options")]
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private DialogueTrigger dialogueTrigger;

    //проверка на столкновение с игроком 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        dialogueTrigger.StartDialogue();
    }

    //создание виртуальной области соприкосновения
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    
    private void StartDialogue()
    {
        if (DialogueManager.isActive)
            dialogueTrigger.StartDialogue();
    }

}
