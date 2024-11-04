using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

namespace Utilities
{
    public class WalletUIHelper : MonoBehaviour
    {
        [SerializeField] private Button bagButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button bgTintButton;
        [SerializeField] private GameObject walletPanel;
        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private WalletSlot walletSlot;

        [SerializeField] private WalletTrailObject outcomeTrail;

        private List<WalletSlot> _pooledSlots = new List<WalletSlot>();
        private int _poolAmount = 20;
        private void Awake()
        {
            PoolWalletSlots();
        }

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        public void AnimateRewardClaim(RewardConfig config, Action onComplete)
        {
            var temp = Instantiate(outcomeTrail, transform.position, Quaternion.identity);
            temp.ConfigureSelf(config);
            temp.AnimateParticle(bagButton.GetComponent<RectTransform>(), () =>
            {
                Destroy(temp.gameObject);
                bagButton.transform.DOShakeScale(0.5f, 0.15f).SetEase(Ease.Linear);
                onComplete?.Invoke();
            });
        }

        private void EnablePanel()
        {
            walletPanel.gameObject.SetActive(true);
            if (Wallet.UIShouldUpdate)
            {
                ResetSlots();
                PopulateRewards();
            }
        }

        private void PopulateRewards()
        {
            var rewards = Wallet.GetPlayerRewards();
            foreach (var reward in rewards)
            {
                var slot = GetAvailableWalletSlot();
                if (slot != null)
                {
                    slot.ConfigureSlot(reward);
                }
            }

            Wallet.UIShouldUpdate = false;
        }

        private void DisablePanel()
        {
            walletPanel.gameObject.SetActive(false);
        }

        private void ResetSlots()
        {
            foreach (var slot in _pooledSlots)
            {
                slot.ResetSlot();
            }
        }
        private void PoolWalletSlots()
        {
            for (int i = 0; i < _poolAmount; i++)
            {
                var tempSlot = Instantiate(walletSlot, gridLayoutGroup.transform);
                _pooledSlots.Add(tempSlot);
            }
        }

        private WalletSlot GetAvailableWalletSlot()
        {
            return _pooledSlots.Find(x => x.IsAvailable());
        }

        private void AddListeners()
        {
            bagButton.onClick.AddListener(EnablePanel);
            closeButton.onClick.AddListener(DisablePanel);
            bgTintButton.onClick.AddListener(DisablePanel);
        }

        private void RemoveListeners()
        {
            bagButton.onClick.RemoveListener(EnablePanel);
            closeButton.onClick.RemoveListener(DisablePanel);
            bgTintButton.onClick.RemoveListener(DisablePanel);
        }
    }
}
