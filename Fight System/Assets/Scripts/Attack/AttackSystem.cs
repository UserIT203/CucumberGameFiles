using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackSystem : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private float timer;

    [Header("Links Cubes")]
    [SerializeField] private Image[] cubes;

    [Header("Links To Menu")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainMenu;

    [Header("State Of Cube")]
    [SerializeField] private Sprite[] stateCube;

    [Header("Links In MainMenu")]
    public Text scoreText;

    [SerializeField] private BattleSystem battleSystem;

    [Header("Sound Links")]
    [SerializeField] private AudioClip diesSound;

    public int summ;
    private bool isSpeen = false;

    private void Start()
    {
        startMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    IEnumerator ChangeSpriteCube()
    {
        if (!isSpeen)
        {
            isSpeen = true;

            System.Random rnd = new System.Random();

            foreach (Image cube in cubes)
            {
                int value = rnd.Next(0, 6);
                cube.GetComponent<Animator>().SetInteger("rollChoise", value);

                Debug.Log(value);

                summ += value + 1;
            }

            yield return new WaitForSeconds(timer);

            ActivateMainMenu();
            isSpeen = false;
            battleSystem.PlayerTurn();

        }
    }

    private void ActivateMainMenu()
    {
        startMenu.SetActive(false);
        mainMenu.SetActive(true);
    }


    public void ActivateStartMenu()
    {
        summ = 0;

        startMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OnSpeen()
    {
        if (battleSystem.state == BattleState.PLAYERSPEEN)
        {
            SoundManager.instance.PlaySound(diesSound);

            StartCoroutine(ChangeSpriteCube());
        }

    }

    private void Update()
    {
        scoreText.text = Convert.ToString(summ) + " score";

        if (Input.GetKeyDown(KeyCode.Space))
            OnSpeen();
    }
}
