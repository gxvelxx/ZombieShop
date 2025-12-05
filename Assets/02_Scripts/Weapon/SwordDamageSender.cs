using UnityEngine;

public class SwordDamageSender : MonoBehaviour
{
    public SwordDamageData damageData;
    public int GetDamage() => damageData.GetRandomDamage();
}
