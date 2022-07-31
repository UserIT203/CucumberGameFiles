using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERSPEEN, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerObject;
    public GameObject enemyObject;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    private Unit playerUnit;
    private Unit enemyUnit;

    public Text dialogueText;

    public ButtleHUD playerHUD;
    public ButtleHUD enemyHUD;

    public BattleState state;

    [SerializeField] private AttackSystem attackSystem;

    [Header("Freeze Options")]
    [SerializeField] private SpriteRenderer playerSpriteRend;

    [Header("Obj Links")]
    [SerializeField] private GameObject hitEnemyObj;
    [SerializeField] private GameObject hitPlayerObj;

    [Header("Animator Links")]
    [SerializeField] private Animator playerAnim;

    [Header("Sounds Links")]
    [SerializeField] private AudioClip playerHitSound;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip attackSound;

    private int turnCount;
    private int currentTurnCount;

    private int freezeTime;
    private int currentFrezzeTime;

    //компоненты врага
    private SpriteRenderer enemySpriteRend;
    private Animator enemyAnim;

    private void Awake()
    {
        enemyObject.transform.GetChild(SystemManager.instance.GetEnemyNum()).gameObject.SetActive(true);

        enemySpriteRend = enemyObject.transform.GetChild(SystemManager.instance.GetEnemyNum()).GetComponent<SpriteRenderer>();
        enemyAnim = enemyObject.transform.GetChild(SystemManager.instance.GetEnemyNum()).GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;

        StartCoroutine(SetupBattle());

        hitEnemyObj.SetActive(false);
        hitPlayerObj.SetActive(false);

        currentTurnCount = 0;
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerObject, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyObject, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "It's  a  " + enemyUnit.unitName;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2F);

        state = BattleState.PLAYERSPEEN;
        PlayerSpeen();
    }

    private void PlayerSpeen()
    {
        dialogueText.text = "Speen  this  cubes \n SPACE";
    }

    IEnumerator PlayerAttack(int _damage)
    {
        bool isDead = enemyUnit.TakeDamage(_damage);

        SoundManager.instance.PlaySound(attackSound);

        playerAnim.SetTrigger("attack");

        SoundManager.instance.PlaySound(enemyHitSound);

        hitEnemyObj.SetActive(true);

        enemyAnim.SetTrigger("hit");

        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.maxHP);
        dialogueText.text = "The  attack  is  successful !";

        enemySpriteRend.color = Color.red;

        //урон по противнику
        yield return new WaitForSeconds(1f);

        if (isAcrivateforkSpell)
            enemySpriteRend.color = Color.green;
        else if (isFreezeEnemy)
            enemySpriteRend.color = Color.blue;
        else
            enemySpriteRend.color = Color.white;

        if (isDead)
        {
            //конец поединка
            state = BattleState.WON;
            EndBattle();
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attack";

        SoundManager.instance.PlaySound(attackSound);

        enemyAnim.SetTrigger("attack");

        SoundManager.instance.PlaySound(playerHitSound);

        hitPlayerObj.SetActive(true);

        playerAnim.SetTrigger("hit");

        playerSpriteRend.color = Color.red;

        yield return new WaitForSeconds(1f);

        if (isFreezePlayer)
            playerSpriteRend.color = Color.blue;
        else
            playerSpriteRend.color = Color.white;

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERSPEEN;
            PlayerSpeen();
        }
    }


    //конец боя
    public bool isWin;
    public bool isLose;
    private void EndBattle()
    {
        //при победе 
        if (state == BattleState.WON)
        {
            isWin = true;
            dialogueText.text = "You won the battle";

            SaveSystem.SavePlayer(playerUnit.unitLevel, playerUnit.damage, playerUnit.maxHP, 
                playerUnit.currentHP, Vector3.zero, true);

            SceneTransition.SwitchToScene("MainScene");
        }
        //при поражение
        else if (state == BattleState.LOST)
        {
            isLose = true;
            dialogueText.text = "You were defeated";
        }
    }

    public void PlayerTurn()
    {
        dialogueText.text = "Choise action:";
        state = BattleState.PLAYERTURN;
    }

    public void OnAttackButton(int _damage)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack(_damage));
    }

    IEnumerator PlayerHeal(int _heal)
    {
        playerUnit.Heal(_heal);

        playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);
        dialogueText.text = "You feel renewed strength";

        yield return new WaitForSeconds(2f);
    }

    public void OnHealButton(int _heal)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal(_heal));
    }

    IEnumerator PlayerAttackAndHeal(int _damage, int _heal)
    {
        bool isDead = enemyUnit.TakeDamage(_damage);

        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.maxHP);
        dialogueText.text = "The attack is successful !";

        enemySpriteRend.color = Color.red;

        //урон по противнику
        yield return new WaitForSeconds(1f);

        if (isAcrivateforkSpell)
            enemySpriteRend.color = Color.green;
        else if (isFreezeEnemy)
            enemySpriteRend.color = Color.blue;
        else
            enemySpriteRend.color = Color.white;

        playerUnit.Heal(_heal);

        playerHUD.SetHP(playerUnit.currentHP, playerUnit.maxHP);
        dialogueText.text = "You feel renewed strength";

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
    }

    public void OnAttackAndHealButton(int _damage, int _heal)
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttackAndHeal(_damage, _heal));
    }

    public void StartEnemyTurn()
    {
        currentTurnCount += 1;
        currentFrezzeTime += 1;

        attackSystem.ActivateStartMenu();

        if (!isFreezeEnemy)
        {
            Debug.Log("Ready");
            StartCoroutine(EnemyTurn());
        }
        else
        {
            Debug.Log("Ready 2");
            state = BattleState.PLAYERSPEEN;
            PlayerSpeen();
        }

        //проверка на отравление
        ForkSpell();

        //проверка на замарозку игрока
        FreezePlayer();

        //проверка на заморозку врага
        FrezzeEnemy();
    }

    //удар вилкой 
    private bool isAcrivateforkSpell = false;

    private void ForkSpell()
    {
        if (isAcrivateforkSpell)
        {
            if (currentTurnCount >= turnCount)
            {
                if (currentTurnCount - turnCount <= 2)
                {
                    bool isDead = enemyUnit.TakeDamage(2);

                    enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.maxHP);

                    if (isDead)
                    {
                        state = BattleState.LOST;
                        EndBattle();
                    }
                }
                else
                {
                    isAcrivateforkSpell = false;

                    if(!isFreezeEnemy)
                        enemySpriteRend.color = Color.white;
                    else
                        enemySpriteRend.color = Color.blue;
                }
            }
        }
    }

    public void OnForkSpell()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        turnCount = currentTurnCount;

        isAcrivateforkSpell = true;
        enemySpriteRend.color = Color.green;

        dialogueText.text = "Poisoning successful";
    }

    //урон от ножа 
    public void OnKnifeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        int value = UnityEngine.Random.Range(0, 50);

        if (value == 0)
            StartCoroutine(PlayerAttack(enemyUnit.currentHP));
        else
            StartCoroutine(PlayerAttack(10));
    }

    //кнопка петрушка
    public bool isFreezePlayer = false;
    public void OnParsleyButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        int value = UnityEngine.Random.Range(1, 100);
        int[] a = new int[5] { 1, 2, 3 , 4, 5 };
        int[] b = new int[47];
        int[] c = new int[47];

        int num = 6;
        for (int i = 0; i < b.Length; i++)
        {
            b[i] = num;
            num += 1;
        }

        for (int i = 0; i < c.Length; i++)
        {
            c[i] = num;
            num += 1;
        }

        if (Array.IndexOf(a, value) != -1)
        {
            isFreezePlayer = true;
            freezeTime = currentFrezzeTime;

            FreezePlayer();
        }
        else if (Array.IndexOf(b, value) != -1)
        {
            isFreezeEnemy = true;
            freezeTime = currentFrezzeTime;

            dialogueText.text = "Enemy freezed";

            FrezzeEnemy();
        }
        else if (Array.IndexOf(c, value) != -1)
        {
            DownDamageEnemy();
        }
    }
    
    //пропуск хода игроком 
    private void FreezePlayer()
    {
        if (isFreezePlayer == false)
            return;

        if (currentFrezzeTime - freezeTime == 0)
        {
            state = BattleState.ENEMYTURN;
            playerSpriteRend.color = Color.blue;
        }
        else
        {
            isFreezePlayer = false;
            playerSpriteRend.color = Color.white;
        }
    }

    IEnumerator DownDamage()
    {
        if (enemyUnit.damage <= 3)
            dialogueText.text = "Enemy is already weak";
        else
        {
            enemyUnit.damage -= 1;

            dialogueText.text = "Enemy damage downgraded";
        }

        yield return new WaitForSeconds(2f);
    }

    //снижение урона противника
    private void DownDamageEnemy()
    {
        StartCoroutine(DownDamage());
    }

    //заморозка врага
    private bool isFreezeEnemy = false;
    private void FrezzeEnemy()
    {
        if (isFreezeEnemy == false)
            return;

        if (currentFrezzeTime - freezeTime == 0)
            enemySpriteRend.color = Color.blue;
        else
        {
            isFreezeEnemy = false;
            if (!isAcrivateforkSpell)
                enemySpriteRend.color = Color.white;
            else
                enemySpriteRend.color = Color.green;
        }
    }
}
