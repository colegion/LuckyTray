using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task<Utility.RewardType> SpinRoulette()
    {
        var outcome = await CloudGateway.GetOutcome();
        return Utility.GetRewardTypeAsEnum(outcome);
    }
}