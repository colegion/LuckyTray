using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

public class InitialSceneController : MonoBehaviour
{
    [SerializeField] private string gameSceneAddress;
    [SerializeField] private Button playButton;

    private async void Awake()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
            await UnityServices.InitializeAsync();
        if(!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Wallet.SetCurrentRoundClaimedRewards(await CloudGateway.GetCurrentRoundClaimedRewards());
        Wallet.FetchUserRewards();
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void RedirectToGame()
    {
        SceneLoader.LoadSceneAsync(SceneType.Game);
    }

    private void AddListeners()
    {
        playButton.onClick.AddListener(RedirectToGame);
    }

    private void RemoveListeners()
    {
        playButton.onClick.RemoveListener(RedirectToGame);
    }
}
