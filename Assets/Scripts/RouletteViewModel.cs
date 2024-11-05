using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

public class RouletteViewModel : MonoBehaviour
{
    [SerializeField] private RouletteModel model;
    [SerializeField] private WalletUIHelper walletUIHelper;
    [SerializeField] private List<Slot> slots;
    [SerializeField] private Button spinButton;
    
    [SerializeField] private float baseDelay;
    
    private Utility.RewardType _lastOutcomeAsEnum;
    private Slot _lastOutcomeAsSlot;
    private List<Slot> _slotsWithoutHighlight = new List<Slot>();
    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    public void DistributeRewardsToSlots(List<RewardConfig> rewards)
    {
        if (slots.Count != rewards.Count)
        {
            Debug.LogError("Count mismatch between slots and rewards! Continuing with list ordering");
        }
        
        var iteration = rewards.Count > slots.Count ? slots.Count : rewards.Count;
        
        for (int i = 0; i < iteration; i++)
        {
            slots[i].ConfigureSelf(rewards[i]);
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].GetClaimedStatus())
            {
                _slotsWithoutHighlight.Add(slots[i]);
            }
        }
    }

    private async void TriggerRoulette()
    {
        ToggleButtonInteractable(false);
        if (_lastOutcomeAsSlot != null)
        {
            _lastOutcomeAsSlot.SetSpriteByState(Utility.SlotStatus.Claimed);
        }
        StartCoroutine(AnimateSlotsProgressively());
        _lastOutcomeAsEnum = await model.SpinRoulette();
        SetLastOutcomeAsSlot();
    }

    private void SetLastOutcomeAsSlot()
    {
        _lastOutcomeAsSlot = slots.Find(slot => slot.GetRewardType() == _lastOutcomeAsEnum);
    }
    
    private IEnumerator AnimateSlotsProgressively()
    {
        var iterationCount = 0;
        while (iterationCount < 2)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if(slots[i].GetClaimedStatus()) continue;
                slots[i].AnimateHighlight();
                yield return new WaitForSeconds(slots[i].GetCurrentDurationForDelay() / 2f);
            }
            iterationCount++;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].GetClaimedStatus()) continue;
            var outcomeIndex = _slotsWithoutHighlight.FindIndex(x => x == _lastOutcomeAsSlot);
            var isNear = i >= outcomeIndex - 3 && i <= outcomeIndex;
            slots[i].AnimateHighlight(isNear, slots[i] == _lastOutcomeAsSlot);
            yield return new WaitForSeconds(slots[i].GetCurrentDurationForDelay());
            if (slots[i] == _lastOutcomeAsSlot)
            {
                walletUIHelper.AnimateRewardClaim(Utility.GetRewardConfigByType(_lastOutcomeAsEnum), () =>
                {
                    model.CheckRoundStatus();
                    ToggleButtonInteractable(true);
                    _slotsWithoutHighlight.Remove(_lastOutcomeAsSlot);
                });
                
                break;
            }
        }
        
        yield return null;
    }

    private void ToggleButtonInteractable(bool toggle)
    {
        spinButton.interactable = toggle;
        spinButton.transition = toggle ? Selectable.Transition.ColorTint : Selectable.Transition.None;
    }

    private void AddListeners()
    {
        spinButton.onClick.AddListener(TriggerRoulette);
    }

    private void RemoveListeners()
    {
        spinButton.onClick.RemoveListener(TriggerRoulette);
    }
}