using DG.Tweening;
using ScriptableObjects.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ResourcesView
{
    public class ResourceViewElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _resourceCountText;
        [SerializeField] private Image _resourceIcon;

        private int _currentResourceCount;
        private ResourceData _resourceData;
        private bool _autoHide;

        public void Init(ResourceData resourceData, bool autoHide)
        {
            _resourceData = resourceData;
            _autoHide = autoHide;
            SetIcon();
            UpdateResourceCount();
        }

        private void SetIcon()
        {
            if (_resourceData == null) return;

            _resourceIcon.sprite = _resourceData.ResourceIcon;
        }

        public void SetCanvasShow(bool force = false)
        {
            gameObject.SetActive(!(_autoHide && !(force || _currentResourceCount > 0)));
        }

        private void ScaleIcon()
        {
            _resourceIcon.transform.DOKill();
            _resourceIcon.transform.DOScale(1.15f, 0.1f).SetEase(Ease.OutQuint)
                .OnComplete(() => _resourceIcon.transform.DOScale(1f, 0.3f));
        }

        private void UpdateResourceCount()
        {
            _currentResourceCount = ResourceHandler.GetResourceCount(_resourceData.Type);
            RefreshText();
        }

        public void AddResource(int value)
        {
            _currentResourceCount += value;
            ScaleIcon();
            RefreshText();
        }

        public void SubtractResource(int value)
        {
            _currentResourceCount -= value;
            ScaleIcon();
            RefreshText();
        }

        public void SetResourceCount(int value)
        {
            _currentResourceCount = value;
            RefreshText();
        }

        private void RefreshText()
        {
            var showCount = Mathf.Clamp(_currentResourceCount, 0, int.MaxValue);
            var text = showCount.ToString();
            _resourceCountText.text = text;
            SetCanvasShow();
        }
    }
}