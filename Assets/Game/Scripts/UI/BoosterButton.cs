using System;
using ScriptableObjects.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Common.Enums;

namespace UI
{
    public class BoosterButton : MonoBehaviour
    {
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Image _upgradeImage;
        [SerializeField] private Image _moneyImage;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _maxLevelText;
        
        private Sprite _buyButtonEnabledSprite;
        private Sprite _buyButtonDisabledSprite;
        private Sprite _upgradeImageEnabled;
        private Sprite _upgradeImageDisabled;

        public UpgradeType UpgradeType => _upgradeType;

        public event Action<UpgradeType> Clicked;

        private void Start() =>
            _buyButton.onClick.AddListener(ButtonClicked);

        public void Initialize(BoosterData boosterData)
        {
            _buyButtonEnabledSprite = boosterData.BuyButtonEnabledSprite;
            _buyButtonDisabledSprite = boosterData.BuyButtonDisabledSprite;
            _upgradeImageEnabled = boosterData.UpgradeImageEnabled;
            _upgradeImageDisabled = boosterData.UpgradeImageDisabled;
        }

        private void ButtonClicked() =>
            Clicked?.Invoke(_upgradeType);

        public void SetCostText(int cost) =>
            _costText.text = cost.ToString();

        public void SetLevelText(int level) =>
            _levelText.text = $"Level {level}";

        public void Toggle(bool enable)
        {
            _buyButton.interactable = enable;
            _buyButton.image.sprite = enable ? _buyButtonEnabledSprite : _buyButtonDisabledSprite;
            _upgradeImage.sprite = enable ? _upgradeImageEnabled : _upgradeImageDisabled;
            _buyButton.image.SetNativeSize();
            _upgradeImage.SetNativeSize();
        }

        public void SetMaxLevel()
        {
            Toggle(false);
            _maxLevelText.gameObject.SetActive(true);
            _moneyImage.gameObject.SetActive(false);
            _costText.gameObject.SetActive(false);
        }

        private void OnDisable() =>
            _buyButton.onClick.RemoveListener(ButtonClicked);
    }
}