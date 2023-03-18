using System;
using System.Collections.Generic;
using ScriptableObjects.Classes;
using UI;
using UI.ResourcesView;
using UnityEngine;
using Utils;
using static Common.Enums;

namespace Gameplay
{
    public class BoosterProvider : Singleton<BoosterProvider>
    {
        public static event Action<UpgradeType> ProgressUpdated;

        [SerializeField] private BoostersData _boostersData;
        [SerializeField] private BoostersUI _boostersUI;

        private static readonly Dictionary<UpgradeType, int> _boostersLevel = new();
        private static readonly Dictionary<UpgradeType, BoosterData> _boostersDataDict = new();

        private void Start()
        {
            foreach (var boosterDataDictionary in _boostersData.Data)
            {
                _boostersLevel[boosterDataDictionary.BoosterType] = 0;
                _boostersDataDict[boosterDataDictionary.BoosterType] = boosterDataDictionary.BoosterData;
            }

            _boostersUI.Initialize(_boostersDataDict);
            _boostersUI.ProgressUpdated += OnButtonClicked;
        }

        public static int GetCost(UpgradeType boosterType)
        {
            var cost = Mathf.CeilToInt(_boostersDataDict[boosterType].BoosterBaseUpgradeCost
                                       * Mathf.Pow(_boostersDataDict[boosterType].BoosterMultiplierCost,
                                           _boostersLevel[boosterType]));
            return cost;
        }

        public static int GetBoosterLevel(UpgradeType boosterType) => _boostersLevel[boosterType];

        public static bool IsBoosterMaxLevel(UpgradeType boosterType) =>
            _boostersLevel[boosterType] == _boostersDataDict[boosterType].BoosterMaxLevel;

        public static float GetBoosterValue(UpgradeType boosterType) =>
            _boostersDataDict[boosterType].BoosterBaseValue +
            _boostersDataDict[boosterType].BoosterMultiplierValue * _boostersLevel[boosterType];

        private void OnButtonClicked(UpgradeType boosterType)
        {
            int costToUpgrade = GetCost(boosterType);
            ResourceHandler.TrySubtractResource(ResourceType.Money, costToUpgrade);
            IncreaseBoosterLevel(boosterType);
            ProgressUpdated?.Invoke(boosterType);
        }

        private void IncreaseBoosterLevel(UpgradeType boosterType)
        {
            _boostersLevel[boosterType]++;
        }
    }
}