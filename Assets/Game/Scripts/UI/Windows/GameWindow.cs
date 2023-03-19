using DG.Tweening;
using QFSW.MOP2;
using TMPro;
using UI.Boosters;
using UnityEngine;

namespace UI.Windows
{
    public class GameWindow : BaseWindow
    {
        [Header("Boosters")]
        [SerializeField] private BoostersUI _boostersUI;
        [Header("Reward")]
        [SerializeField] private RewardUI _rewardPrefab;
        [Header("Move To Play Settings")]
        [SerializeField] private TMP_Text _moveToPlayText;
        [SerializeField] private float _moveToPlayScaleSize;
        [SerializeField] private float _moveToPlayScaleDuration;
        private Vector3 _cachedMoveToPlayScale;
        private ObjectPool _rewardPool;

        private void Awake()
        {
            _cachedMoveToPlayScale = _moveToPlayText.transform.localScale;
            _rewardPool = ObjectPool.Create(_rewardPrefab.gameObject);
            _rewardPool.ObjectParent.SetParent(transform);
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

        public void ShowReward(Vector2 position, int reward)
        {
            var rewardUI = _rewardPool.GetObjectComponent<RewardUI>();
            rewardUI.Show(position, reward, 
                () => _rewardPool.Release(rewardUI.gameObject));
        }

        public void ShowBoosters() => _boostersUI.Show();
        public void HideBoosters() => _boostersUI.Hide();
    }
}