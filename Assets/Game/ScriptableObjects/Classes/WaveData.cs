using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(fileName = "New Wave Data", menuName = "Wave Data")]
    public class WaveData: ScriptableObject
    {
        [field: SerializeField] public List<EnemyChanceData> EnemyChanceDatas { get; private set; }
        [field: SerializeField, Min(0.1f)] public float MinDelayBeforeSpawn { get; private set; }
        [field: SerializeField, Min(0.1f)] public float MaxDelayBeforeSpawn { get; private set; }
    }

    [Serializable]
    public class EnemyChanceData
    {
        public Enums.EnemyType Type;
        [Range(0,100)] public float Probability;
    }
}