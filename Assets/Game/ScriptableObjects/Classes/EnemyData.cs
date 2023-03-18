using UnityEngine;
using static Common.Enums;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int Reward { get; private set; }
    }
}