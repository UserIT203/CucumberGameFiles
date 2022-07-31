using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Characteristics")]
    [SerializeField] private string name;
    [SerializeField] private int damage;
    [SerializeField] private int maxHP;
    public bool isDead;

    [Header("Enemy Type")]
    [SerializeField] private bool tomato;
    [SerializeField] private bool carroy;
    
    [Header("Collisions Options")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Transform playerMovement;

    [SerializeField] private DialogueTrigger dialogueTrigger;

    private Transform markTriggerSprite;

    private void Awake()
    {
        markTriggerSprite = this.transform.GetChild(0).GetComponent<Transform>();
        markTriggerSprite.localScale = Vector3.zero;
    }

    //проверка на столкновение с игроком 
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center,
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        //устанока характеристик игрока
        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            SystemManager.instance.enemyObj = this.transform.GetComponent<Enemy>();

            Player playerStat = hit.collider.gameObject.GetComponent<Player>();

            SystemManager.instance._maxHPPlayer = playerStat.maxHP;
            SystemManager.instance._currentHPPlayer = playerStat.currentHP;
            SystemManager.instance._damagePlayer = playerStat.damage;
            SystemManager.instance._levelPlayer = playerStat.level;

            playerStat.SavePlayer();
        }

        return hit.collider != null;
    }

    //создание виртуальной области соприкосновения
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center,
            new Vector3(boxCollider.bounds.size.x, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    IEnumerator MarkTrigger()
    {
        markTriggerSprite.LeanScale(new Vector3(2f, 2f, 2f), 0.5f).setEaseInOutExpo();
        yield return new WaitForSeconds(1f);
        markTriggerSprite.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();

        dialogueTrigger.StartDialogue();
    }

    private void TriggerForPlayer()
    {
        if (!playerMovement.GetComponent<PlayerMovement>().isAttacked)
        {
            StartCoroutine(MarkTrigger());
        }

        playerMovement.GetComponent<PlayerMovement>().isAttacked = true;

        Vector3 playerPosition = new Vector3(playerMovement.position.x - 1f, playerMovement.position.y, playerMovement.position.z);
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, 4 * Time.deltaTime);
    }

    //переброс на сцену с боевой системой 
    public void Fight()
    {
        if (tomato)
        {
            SystemManager.instance.enemyType[1] = false;

            SystemManager.instance.enemyType[0] = true;
        }
        else if (carroy)
        {
            SystemManager.instance.enemyType[0] = false;

            SystemManager.instance.enemyType[1] = true;
        }


        SystemManager.instance._nameEnemy = name;
        SystemManager.instance._damageEnemy = damage;
        SystemManager.instance._maxHPEnemy = maxHP;



        SceneTransition.SwitchToScene("FightSystemScene");
    }

    private void Update()
    {
        if (isDead)
            Destroy(gameObject);

        if (PlayerInSight())
            TriggerForPlayer();
    }
}
