using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

public class RouletteViewModel : MonoBehaviour
{
    [SerializeField] private RouletteModel model;
    [SerializeField] private WalletUIHelper walletUIHelper;
    [SerializeField] private string slotsLabel;
    [SerializeField] private Button spinButton;
    
    [SerializeField] private float slotConfigureDelay;
    
    private Utility.RewardType _lastOutcomeAsEnum;
    private Slot _lastOutcomeAsSlot;
    private List<Slot> _slots;
    private readonly List<Slot> _slotsWithoutHighlight = new List<Slot>();

    public void LoadSlots()
    {
        Addressables.LoadAssetsAsync<GameObject>(slotsLabel, null).Completed += OnRewardsLoaded;
    }

    private void OnRewardsLoaded(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var parent = handle.Result[0];
            var parentInstance = Instantiate(parent, parent.transform.position, Quaternion.identity);
            var children = parentInstance.GetComponentsInChildren<Slot>();
            _slots = new List<Slot>(children);
            DistributeRewardsToSlots(model.GetRewards());
        }
        else
        {
            Debug.LogWarning("Could not load game slots!");
            _slots = new List<Slot>();
        }
    }

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
        if (_slots.Count != rewards.Count)
        {
            Debug.LogError("Count mismatch between slots and rewards! Continuing with list ordering");
        }
        
        var iteration = rewards.Count > _slots.Count ? _slots.Count : rewards.Count;
        rewards.Shuffle();
        for (int i = 0; i < iteration; i++)
        {
            _slots[i].ConfigureSelf(rewards[i], i * slotConfigureDelay);
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if (!_slots[i].GetClaimedStatus())
            {
                _slotsWithoutHighlight.Add(_slots[i]);
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
        _lastOutcomeAsSlot = _slots.Find(slot => slot.GetRewardType() == _lastOutcomeAsEnum);
    }
    
    private IEnumerator AnimateSlotsProgressively()
    {
        var iterationCount = 0;
        while (iterationCount < 2)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if(_slots[i].GetClaimedStatus()) continue;
                _slots[i].AnimateHighlight();
                yield return new WaitForSeconds(_slots[i].GetCurrentDurationForDelay() / 2f);
            }
            iterationCount++;
        }

        for (int i = 0; i < _slots.Count; i++)
        {
            if(_slots[i].GetClaimedStatus()) continue;
            var outcomeIndex = _slotsWithoutHighlight.FindIndex(x => x == _lastOutcomeAsSlot);
            var isNear = i >= outcomeIndex - 3 && i <= outcomeIndex;
            _slots[i].AnimateHighlight(isNear, _slots[i] == _lastOutcomeAsSlot);
            yield return new WaitForSeconds(_slots[i].GetCurrentDurationForDelay());
            if (_slots[i] == _lastOutcomeAsSlot)
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