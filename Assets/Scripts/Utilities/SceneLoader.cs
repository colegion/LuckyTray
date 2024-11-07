using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Utilities
{
    public static class SceneLoader
    {
        private static Dictionary<SceneType, string> _sceneAddresses = new Dictionary<SceneType, string>()
        {
            { SceneType.Intro , "Scenes/InitialScene" },
            { SceneType.Game , "Scenes/BarbecueParty" }
        };
        
        public static void LoadSceneAsync(SceneType type)
        {
            Addressables.LoadSceneAsync(_sceneAddresses[type]).Completed += OnSceneLoaded;
        }
        
        private static void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Game scene loaded successfully!");
            }
            else
            {
                Debug.LogWarning("Failed to load game scene.");
            }
        }
    }
    
    public enum SceneType
    {
        Intro,
        Game,
    }
}
