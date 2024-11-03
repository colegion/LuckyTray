using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

public class RouletteViewModel : MonoBehaviour
{
    [FormerlySerializedAs("controller")] [SerializeField] private RouletteModel model;
    [SerializeField] private List<Slot> slots;
    [SerializeField] private Button spinButton;


    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
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

    private void TriggerRoulette()
    {
        model.SpinRoulette();
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