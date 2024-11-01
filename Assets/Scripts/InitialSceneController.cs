using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitialSceneController : MonoBehaviour
{
    [SerializeField] private Button playButton;

    private readonly string _gameSceneName = "BarbecuePartyScene";

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
        SceneManager.LoadScene(_gameSceneName);
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
