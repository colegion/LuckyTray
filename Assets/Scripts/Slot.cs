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
    [SerializeField] private float baseHighlightDuration;
    [SerializeField] private AnimationCurve baseHighlightCurve;
    [SerializeField] private float finalHighlightDuration;
    [SerializeField] private AnimationCurve finalHighlightCurve;
    [SerializeField] private float outcomeHighlightDuration;
    [SerializeField] private AnimationCurve outcomeHighlightCurve;


    private RewardConfig _config;
    public void ConfigureSelf(RewardConfig config)
    {
        _config = config;
        rewardField.sprite = config.rewardSprite;
    }

    private float _durationToUse;
    public void AnimateHighlight(bool isFinal = false, bool isOutcome = false)
    {
        var curveToUse = isOutcome && isFinal ? outcomeHighlightCurve :
            isFinal ? finalHighlightCurve : baseHighlightCurve;
        _durationToUse = isOutcome && isFinal ? outcomeHighlightDuration :
            isFinal ? finalHighlightDuration : baseHighlightDuration;
        
            slotHighlight.enabled = true;
            slotHighlight.DOColor(new Color(1, 1, 1, 1), _durationToUse).SetEase(curveToUse).OnComplete(() =>
            {
                slotHighlight.DOColor(new Color(1, 1, 1, 0), _durationToUse).OnComplete(() =>
                {
                    slotHighlight.enabled = false;
                });
            });
    }

    public float GetCurrentDurationForDelay()
    {
        return _durationToUse;
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
