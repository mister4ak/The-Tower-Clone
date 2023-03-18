using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace ScriptableObjects.Classes
{
    [CreateAssetMenu(fileName = "NewBoostersData", menuName = "Boosters Data")]
    public class BoostersData : ScriptableObject
    {
        [SerializeField] private List<BoosterDataDictionary> _boostersData;
        
        public List<BoosterDataDictionary> Data => _boostersData;
    }
    
    [Serializable]
    public class BoosterDataDictionary
    {
        public Enums.UpgradeType BoosterType;
        public BoosterData BoosterData;
    }

    [Serializable]
    public class BoosterData
    {
        public int BoosterBaseUpgradeCost;
        public float BoosterMultiplierCost;
        public float BoosterBaseValue;
        public float BoosterMultiplierValue;
        public int BoosterMaxLevel;
        public Sprite BuyButtonEnabledSprite;
        public Sprite BuyButtonDisabledSprite;
        public Sprite UpgradeImageEnabled;
        public Sprite UpgradeImageDisabled;
    }
}