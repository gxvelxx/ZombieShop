using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPointData", menuName = "Scriptable Objects/SpawnPointData")]
public class SpawnPointData : ScriptableObject
{
    public float _radius = 60f;
    public int _maxCount = 10;

    [HideInInspector]
    public int _currentCount = 0;
}
