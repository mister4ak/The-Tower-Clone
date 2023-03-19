using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RewardUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _deltaPositionY;
        [SerializeField] private float _moveDuration;

        public void Show(Vector2 position, int reward, Action finished = null)
        {
            UpdatePosition(position);
            UpdateText(reward);
            RunMoveTween(finished);
        }

        private void UpdatePosition(Vector2 position) => transform.position = position;
        
        private void UpdateText(int reward) => _rewardText.text = $"+{reward}";
        
        private void RunMoveTween(Action finished)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1f, _fadeInDuration));
            sequence.Append(transform.DOMoveY(transform.position.y + _deltaPositionY, _moveDuration));
            sequence.Join(_canvasGroup.DOFade(0f, _moveDuration));
            sequence.AppendCallback(() =>
            {
                finished?.Invoke();
            });
        }
    }
}