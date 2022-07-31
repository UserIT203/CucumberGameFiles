using UnityEngine;

public class Player : MonoBehaviour
{
    public string level;
    public int damage;
    public int maxHP;
    public int currentHP;

    private void Awake()
    {
        LoadPlayer();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(level, damage, maxHP,
            currentHP, this.transform.position);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        level = data.level;
        damage = data.damage;
        maxHP = data.maxHealth;
        currentHP = data.currentHealth;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
}
