using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string level;
    public int damage;
    public int maxHealth;
    public int currentHealth;
    public float[] position;

    public PlayerData(string _playerLevel, int _playerDamage, int _playerMaxHP, 
        int _playerCurrentHP, Vector3 _playerPosition, bool inFightScene = false)
    {
        level = _playerLevel;
        damage = _playerDamage;
        maxHealth = _playerMaxHP;
        currentHealth = _playerCurrentHP;

        if (!inFightScene)
        {
            position = new float[3];
            position[0] = _playerPosition.x;
            position[1] = _playerPosition.y;
            position[2] = _playerPosition.z;
        }

    }
}
