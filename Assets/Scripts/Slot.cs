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
    [SerializeField] private float scaleValue;
    [SerializeField] private float baseHighlightDuration;
    [SerializeField] private AnimationCurve baseHighlightCurve;
    [SerializeField] private float finalHighlightDuration;
    [SerializeField] private AnimationCurve finalHighlightCurve;
    [SerializeField] private float outcomeHighlightDuration;
    [SerializeField] private AnimationCurve outcomeHighlightCurve;

    private readonly Color _fullAlpha = new Color(1, 1, 1, 1);
    private readonly Color _zeroAlpha = new Color(1, 1, 1, 0);
    
    private RewardConfig _config;
    private bool _alreadyClaimed;
    public void ConfigureSelf(RewardConfig config, float delay)
    {
        _config = config;
        DOVirtual.DelayedCall(delay, () =>
        {
            transform.DOScale(new Vector3(scaleValue, scaleValue, scaleValue), baseHighlightDuration).SetEase(Ease.InCubic).OnComplete(() =>
            {
                rewardField.sprite = config.rewardSprite;
                rewardField.DOColor(_fullAlpha, baseHighlightDuration).SetEase(Ease.Flash);
                if (Wallet.IsRewardAlreadyClaimed((int)_config.rewardType))
                {
                    SetSpriteByState(Utility.SlotStatus.Claimed);
                }
            });
        });
    }

    private float _durationToUse;
    public void AnimateHighlight(bool nearEnough = false, bool isOutcome = false)
    {
        var curveToUse = isOutcome && nearEnough ? outcomeHighlightCurve :
            nearEnough ? finalHighlightCurve : baseHighlightCurve;
        _durationToUse = isOutcome && nearEnough ? outcomeHighlightDuration :
            nearEnough ? finalHighlightDuration : baseHighlightDuration;
        
            slotHighlight.enabled = true;
            slotHighlight.DOColor(_fullAlpha, _durationToUse).SetEase(curveToUse).OnComplete(() =>
            {
                slotHighlight.DOColor(_zeroAlpha, _durationToUse).OnComplete(() =>
                {
                    if (isOutcome)
                    {
                        AnimateOutcomeHighlight();
                    }
                    else
                    {
                        slotHighlight.enabled = false;
                    }
                });
            });
    }

    private void AnimateOutcomeHighlight()
    {
        Sequence sequence = DOTween.Sequence();
        
        SetSpriteByState(Utility.SlotStatus.Granted);
        sequence.Append(slotHighlight.DOColor(_fullAlpha, baseHighlightDuration / 2f))
            .Append(slotHighlight.DOColor(_zeroAlpha, baseHighlightDuration / 2f))
            .SetLoops(3, LoopType.Yoyo);

        sequence.OnComplete(() =>
        {
            tickSprite.enabled = true;
            tickSprite.DOColor(_fullAlpha, 0.5f);
        });
    }
    
    public float GetCurrentDurationForDelay()
    {
        return _durationToUse;
    }

    public void SetSpriteByState(Utility.SlotStatus status)
    {
        slotBg.sprite = states.Find(x => x.slotStatus == status).statusSprite;
        tickSprite.enabled = status == Utility.SlotStatus.Claimed;
        _alreadyClaimed = status == Utility.SlotStatus.Claimed;
    }

    public bool GetClaimedStatus()
    {
        return _alreadyClaimed;
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
