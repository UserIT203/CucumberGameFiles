using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string speelName;

    [TextArea(3, 10)]
    [SerializeField] private string descriptions;

    [Header("Type Of Spell")]
    [SerializeField] private bool justDamage;
    [SerializeField] private bool justHeal;
    [SerializeField] private bool justDamageAndHeal;
    [SerializeField] private bool forkSpeel;
    [SerializeField] private bool knifeSpeel;
    [SerializeField] private bool parsleySpeel;

    [Header("Options")]
    [SerializeField] public int damageButton;
    [SerializeField] public int healForce;
    [SerializeField] private int scoreSumm;

    [SerializeField] private Text buttonText;

    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private AttackSystem attackSystem;

    [Header("Sound Links")]
    [SerializeField] private AudioClip choiseSound;

    public RectTransform fingerRect;

    private void Start()
    {
        buttonText.text = speelName;
    }

    public void ActivateButton()
    {
        if (battleSystem.isFreezePlayer || battleSystem.state == BattleState.WON 
            || battleSystem.state == BattleState.LOST)
            return;

        if (scoreSumm > attackSystem.summ)
        {
            battleSystem.dialogueText.text = "You BOMJ. Chosee another";
            return;
        }

        if (justDamage)
            battleSystem.OnAttackButton(damageButton);
        else if (justHeal)
            battleSystem.OnHealButton(healForce);
        else if (justDamageAndHeal)
            battleSystem.OnAttackAndHealButton(damageButton, healForce);
        else if (forkSpeel)
            battleSystem.OnForkSpell();
        else if (knifeSpeel)
            battleSystem.OnKnifeButton();
        else if (parsleySpeel)
            battleSystem.OnParsleyButton();

        attackSystem.summ -= scoreSumm;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        SoundManager.instance.PlaySound(choiseSound);

        if (fingerRect != null)
            fingerRect.position = new Vector3(transform.position.x - 175f, transform.position.y, transform.position.z);

        if (battleSystem.isFreezePlayer || battleSystem.isWin || battleSystem.isLose)
            return;

        battleSystem.dialogueText.text = descriptions;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (battleSystem.isFreezePlayer)
            battleSystem.dialogueText.text = "You freeze";
        else if (battleSystem.isWin)
            battleSystem.dialogueText.text = "You won the battle";
        else if (battleSystem.isLose)
            battleSystem.dialogueText.text = "You were defeated";
        else
            battleSystem.dialogueText.text = "Choose an action:";
    }

}
