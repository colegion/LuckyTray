using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class RouletteModel : MonoBehaviour
{
    [SerializeField] private RouletteViewModel rouletteViewModel;

    private void Start()
    {
        InitiateRoulette();
    }

    private void InitiateRoulette()
    {
        rouletteViewModel.DistributeRewardsToSlots(Utility.GetRewards());
    }

    public async Task<Utility.RewardType> SpinRoulette()
    {
        var outcome = await CloudGateway.GetOutcome();
        var outcomeAsEnum = Utility.GetRewardTypeAsEnum(outcome);
        Wallet.AddReward(outcomeAsEnum);
        return outcomeAsEnum;
    }
}