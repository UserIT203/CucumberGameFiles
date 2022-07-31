using UnityEngine;
using UnityEngine.UI;

public class ButtleHUD : MonoBehaviour
{
    [Header("Links HeartObj")]
    [SerializeField] private Image heartImage;

    [Header("Links For Heart")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite brokeHeartSprite;

    public Text nameText;
    public Text levelText;
    public Text HPtext;

    public Slider hpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;

        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;

        HPtext.text = unit.currentHP + " / " + unit.maxHP;

        heartImage.sprite = fullHeartSprite;
    }

    public void SetHP(int hp, int maxHP)
    {
        if (hp < 0) hp = 0;

        hpSlider.value = hp;

        HPtext.text = hp + " / " + maxHP;

        if ((float)hp / maxHP < 0.5)
            heartImage.sprite = brokeHeartSprite;
        else
            heartImage.sprite = fullHeartSprite;
    }
}
