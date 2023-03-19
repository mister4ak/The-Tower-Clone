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
        [field: SerializeField, Min(0.1f)] public float WaveDuration { get; private set; }
        [field: SerializeField, Min(0.1f)] public float StartMinSpawnTimer { get; private set; }
        [field: SerializeField, Min(0.1f)] public float StartMaxSpawnTimer { get; private set; }
        [field: SerializeField, Min(0.1f)] public float DecreaseSpawnSpeedPerWave { get; private set; }
        [field: SerializeField, Min(0.1f)] public float MinSpawnTimeInGame { get; private set; }
    }

    [Serializable]
    public class EnemyChanceData
    {
        public Enums.EnemyType Type;
        [Range(0,100)] public float Probability;
    }
}