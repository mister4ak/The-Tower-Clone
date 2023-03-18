using System;
using System.Collections.Generic;
using Extensions;
using Providers;
using ScriptableObjects.Classes;
using UI.ResourcesView;
using UnityEngine;
using static Common.Enums;

namespace UI
{
    public class BoostersUI : MonoBehaviour
    {
        public event Action<UpgradeType> ProgressUpdated;
    
        [Header("References")] 
        [SerializeField] private List<BoosterButton> _boosters;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Header("Other")] 
        [SerializeField] private float _fadeDuration = 0.25f;

        private readonly Dictionary<UpgradeType, BoosterButton> _boosterButtons = new();

        public void Initialize(Dictionary<UpgradeType, BoosterData> boostersDataDict)
        {
            foreach (var booster in _boosters)
            {
                booster.Initialize(boostersDataDict[booster.UpgradeType]);
                booster.Clicked += OnButtonClicked;
                _boosterButtons.Add(booster.UpgradeType, booster);
            }

            ResourceHandler.OnValueChanged += (_, __) => ChangeButtonsState();
            InitializeButtons();
        }

        public void Show() => _canvasGroup.Show(_fadeDuration);
        public void Hide() => _canvasGroup.Hide(_fadeDuration);

        private void InitializeButtons()
        {
            foreach (var boosterButton in _boosterButtons)
            {
                var boosterType = boosterButton.Key;
                if (BoosterProvider.IsBoosterMaxLevel(boosterType))
                {
                    boosterButton.Value.SetMaxLevel();
                    continue;
                }
                UpdateBoosterButtonText(boosterType);
            }

            ChangeButtonsState();
        }

        private void UpdateBoosterButtonText(UpgradeType boosterType)
        {
            if (!_boosterButtons.TryGetValue(boosterType, out BoosterButton boosterButton)) return;
            
            boosterButton.SetCostText(BoosterProvider.GetCost(boosterType));
            boosterButton.SetLevelText(BoosterProvider.GetBoosterLevel(boosterType));
        }

        private void ChangeButtonsState()
        {
            foreach (var (boosterType, boosterButton) in _boosterButtons)
            {
                if (BoosterProvider.IsBoosterMaxLevel(boosterType))
                {
                    boosterButton.SetMaxLevel();
                    continue;
                }

                if (BoosterProvider.GetCost(boosterType) > ResourceHandler.GetResourceCount(ResourceType.Money))
                    boosterButton.Toggle(false);
                else
                    boosterButton.Toggle(true);
            }
        }

        private void OnButtonClicked(UpgradeType boosterType)
        {
            ProgressUpdated?.Invoke(boosterType);
            UpdateBoosterButtonText(boosterType);
            ChangeButtonsState();
        }

        private void OnDisable()
        {
            foreach (var booster in _boosters) 
                booster.Clicked -= OnButtonClicked;
        }
    }
}