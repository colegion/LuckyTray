using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class RouletteModel : MonoBehaviour
{
    [SerializeField] private RouletteViewModel rouletteViewModel;
    [SerializeField] private List<RewardConfig> rouletteRewards;

    private void OnEnable()
    {
        InitiateRoulette();
    }

    private void InitiateRoulette()
    {
        rouletteViewModel.DistributeRewardsToSlots(rouletteRewards);
    }

    public void SpinRoulette()
    {
        
    }
}