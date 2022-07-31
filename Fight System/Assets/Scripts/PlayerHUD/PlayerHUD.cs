using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private Slider HPBar;
    [SerializeField] private Player playerStat;

    private void Awake()
    {
        HPBar.maxValue = playerStat.maxHP;
        HPBar.value = playerStat.currentHP;
    }

    private void Update()
    {
        HPBar.value = playerStat.currentHP;
    }
}
