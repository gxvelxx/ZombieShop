using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModelData", menuName = "Scriptable Objects/PlayerModelData")]
public class PlayerModelData : ScriptableObject
{
    [Header("Player Setting")]
    public int _maxHP = 100;
    public int _currentHP = 1;

    public void Initialize()
    {
        _currentHP = _maxHP;
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
    }

    public bool IsDead()
    {
        return _currentHP <= 0;
    }
}
