using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    public static SystemManager instance { get; private set; }

    public Dictionary<int, bool> enemyType = new Dictionary<int, bool>() 
    {
        { 0, false }, //tomato
        { 1, false } //carroy
    };

    public Enemy enemyObj;

    public bool enemyDie = false;

    //характеристика врагов
    public string _nameEnemy;
    public int _damageEnemy;
    public int _maxHPEnemy;

    //характеристика игрока
    public string _levelPlayer;
    public int _damagePlayer;
    public int _currentHPPlayer;
    public int _maxHPPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        Debug.Log("Start");
    }

    //получение номера типа врага 
    public int GetEnemyNum()
    {
        for (int i = 0; i < enemyType.Count; i++)
        {
            if (enemyType[i] == true)
                return i;
        }
        return 0;
    }

    //установка характеристик врага
    public void SetEnemyCharacteristics()
    {
        Unit enemyUnit = GameObject.Find("Enemy").GetComponent<Unit>();

        enemyUnit.unitName = _nameEnemy; 
        enemyUnit.damage = _damageEnemy;
        enemyUnit.maxHP = _maxHPEnemy;
        enemyUnit.currentHP = _maxHPEnemy;
    }

    public void SetPlayerCharacteristics()
    {
        Unit playerUnit = GameObject.Find("Player").GetComponent<Unit>();

        playerUnit.damage = _damagePlayer;
        playerUnit.maxHP = _maxHPPlayer;
        playerUnit.currentHP = _currentHPPlayer;
        playerUnit.unitLevel = _levelPlayer;
    }
}

