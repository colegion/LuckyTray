using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utilities;

public class Slot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer slotBg;
    [SerializeField] private SpriteRenderer rewardField;
    [SerializeField] private SpriteRenderer slotHighlight;
    [SerializeField] private List<SlotState> states;
    [SerializeField] private SpriteRenderer tickSprite;

    [Header("Tween Settings")] 
    [SerializeField] private float highlightDuration;


    private RewardConfig _config;
    public void ConfigureSelf(RewardConfig config)
    {
        _config = config;
        rewardField.sprite = config.rewardSprite;
    }

    public void AnimateHighlight()
    {
            slotHighlight.enabled = true;
            slotHighlight.DOColor(new Color(1, 1, 1, 1), .35f).SetEase(Ease.Linear).OnComplete(() =>
            {
                slotHighlight.DOColor(new Color(1, 1, 1, 0), .35f).OnComplete(() =>
                {
                    slotHighlight.enabled = false;
                });
                
            });
    }

    public void HandleOnSlotGranted(Utility.SlotStatus status)
    {
        tickSprite.enabled = true;
        SetSpriteByState(status);
    }

    public void SetSpriteByState(Utility.SlotStatus status)
    {
        slotBg.sprite = states.Find(x => x.slotStatus == status).statusSprite;
    }
    
    public Utility.RewardType GetRewardType()
    {
        return _config.rewardType;
    }
    
    [Serializable]
    public class SlotState
    {
        public Utility.SlotStatus slotStatus;
        public Sprite statusSprite;
    }
}
