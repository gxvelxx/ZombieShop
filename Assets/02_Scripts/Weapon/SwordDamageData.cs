using UnityEngine;

[CreateAssetMenu(fileName = "SwordDamageData", menuName = "Scriptable Objects/SwordDamageData")]
public class SwordDamageData : ScriptableObject
{
    [Header("Damage %")]
    [Range(0, 100)] public int damageSet1 = 60;
    [Range(0, 100)] public int damageSet2 = 30;
    [Range(0, 100)] public int damageSet3 = 10;

    [Header("Damage Value")]
    public int damage1 = 1;
    public int damage2 = 2;
    public int damage3 = 3;

    public int GetRandomDamage()
    {
        int roll = Random.Range(0, 100);

        if (roll < damageSet1)
            return damage1;
        else if (roll < damageSet1 + damageSet2)
            return damage2;
        else
            return damage3;
    }
}
