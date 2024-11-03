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
    [SerializeField] private List<Slot> slots;
    [SerializeField] private Button spinButton;

    [SerializeField] private float baseDelay;

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
    }

    public async void TriggerRoulette()
    {
        _highlightCoroutine = StartCoroutine(AnimateSlotsProgressively());
        var rewardType = await model.SpinRoulette();
        
        StopCoroutine(_highlightCoroutine);
        foreach (var slot in slots)
        {
            if (slot.GetRewardType() == rewardType)
            {
                slot.AnimateHighlight(0f);
            }
        }
    }

    private Coroutine _highlightCoroutine;
    private IEnumerator AnimateSlotsProgressively()
    {
        while (true)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].AnimateHighlight(0f);
                yield return new WaitForSeconds(baseDelay);
            }
        }

        yield return null;
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