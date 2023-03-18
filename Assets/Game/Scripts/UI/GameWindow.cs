using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameWindow : BaseWindow
    {
        [Header("Move To Play Settings")]
        [SerializeField] private TMP_Text _moveToPlayText;
        [SerializeField] private float _moveToPlayScaleSize;
        [SerializeField] private float _moveToPlayScaleDuration;
        private Vector3 _cachedMoveToPlayScale;

        private void Awake()
        {
            _cachedMoveToPlayScale = _moveToPlayText.transform.localScale;
        }

        public void ShowMoveToPlay()
        {
            _moveToPlayText.gameObject.SetActive(true);
            _moveToPlayText.transform.localScale = _cachedMoveToPlayScale;
            _moveToPlayText.transform.DOScale(_moveToPlayScaleSize, _moveToPlayScaleDuration)
                .SetLoops(-1, LoopType.Yoyo);
        }

        public void HideMoveToPlay()
        {
            _moveToPlayText.transform.DOKill();
            _moveToPlayText.gameObject.SetActive(false);
        }
    }
}