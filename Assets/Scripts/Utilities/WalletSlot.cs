using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
    public class WalletSlot : MonoBehaviour
    {
        [SerializeField] private Image rewardField;
        [SerializeField] private TextMeshProUGUI rewardCount;
        [SerializeField] private Button slotButton;

        private bool _isAvailable = true;
        private void OnEnable()
        {
           AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        public void ConfigureSlot(KeyValuePair<Utility.RewardType, int> reward)
        {
            rewardField.sprite = Utility.GetRewardConfigByType(reward.Key).rewardSprite;
            rewardField.enabled = true;
            rewardCount.text = $"{reward.Value}";
            _isAvailable = false;
        }

        public void ResetSlot()
        {
            rewardField.sprite = null;
            rewardField.enabled = false;
            rewardCount.text = "";
        }

        private void AnimateSlot()
        {
            transform.DOShakeScale(0.5f, 0.2f).SetEase(Ease.OutCubic);
        }

        public bool IsAvailable()
        {
            return _isAvailable;
        }

        private void AddListeners()
        {
            slotButton.onClick.AddListener(AnimateSlot);
        }

        private void RemoveListeners()
        {
            slotButton.onClick.RemoveListener(AnimateSlot);
        }
    }
}
