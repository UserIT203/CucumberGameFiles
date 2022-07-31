using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public string unitLevel;

    public int damage;

    public int maxHP;
    public int currentHP;

    private void Awake()
    {
        SystemManager.instance.SetEnemyCharacteristics();
        SystemManager.instance.SetPlayerCharacteristics();
    }

    public bool TakeDamage(int _damage)
    {
        currentHP -= _damage;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
}
