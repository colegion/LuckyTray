using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class RouletteModel : MonoBehaviour
{
    [SerializeField] private RouletteViewModel rouletteViewModel;

    private readonly string _initialSceneName = "InitialScene";

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private List<RewardConfig> _rewards; 
    private void InitiateRoulette(List<RewardConfig> configs)
    {
        _rewards = configs;
        rouletteViewModel.LoadSlots();
    }

    public List<RewardConfig> GetRewards()
    {
        return _rewards;
    }

    public async Task<Utility.RewardType> SpinRoulette()
    {
        var outcome = await CloudGateway.GetOutcome();
        var outcomeAsEnum = Utility.GetRewardTypeAsEnum(outcome);
        Wallet.AddReward(outcomeAsEnum);
        return outcomeAsEnum;
    }

    public async void CheckRoundStatus()
    {
        var roundCompleted = await CloudGateway.IsRoundFinished();
        if (roundCompleted)
        {
            SceneManager.LoadScene(_initialSceneName);
        }
    }

    private void AddListeners()
    {
        Utility.OnRewardConfigsLoaded += InitiateRoulette;
    }

    private void RemoveListeners()
    {
        Utility.OnRewardConfigsLoaded -= InitiateRoulette;
    }
}