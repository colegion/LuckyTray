using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utilities
{
    public class Utility : MonoBehaviour
    {
        [SerializeField] private string rewardsAddress;
        private static List<RewardConfig> _rewards;
        public static event Action<List<RewardConfig>> OnRewardConfigsLoaded;
        private void Awake()
        {
            LoadRewards();
        }

        private void LoadRewards()
        {
            Addressables.LoadAssetsAsync<RewardConfig>(rewardsAddress, null).Completed += OnRewardsLoaded;
        }

        private void OnRewardsLoaded(AsyncOperationHandle<IList<RewardConfig>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _rewards = new List<RewardConfig>(handle.Result);
                OnRewardConfigsLoaded?.Invoke(_rewards);
            }
            else
            {
                Debug.LogWarning("Failed to load rewards from Addressables, using default list.");
                _rewards = new List<RewardConfig>();
            }
        }

        public static RewardType GetRewardTypeAsEnum(int outcome)
        {
            var type = (RewardType)outcome;
            return type;
        }

        public static RewardConfig GetRewardConfigByType(RewardType type)
        {
            return _rewards.Find(x => x.rewardType == type);
        }
        
        public enum RewardType
        {
            Beer = 0,
            Candy,
            Coconut,
            Croissant,
            Donut,
            Egg,
            Fig,
            GodFood,
            HotChocolate,
            HotDog,
            Mushroom,
            Noodle,
            Pineapple,
            Shrimp,
        }

        public enum SlotStatus
        {
            Available = 0,
            Granted,
            Claimed,
        }


        [Serializable]
        public class Outcome
        {
            public int outcome;
        }

        [Serializable]
        public class ClaimedRewards
        {
            public int[] rewards;
        }

        [Serializable]
        public class RoundStatus
        {
            [JsonProperty(propertyName: "roundFinished")] public bool isFinished;
        }
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count - 1; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
