using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Utilities
{
    public class Utility : MonoBehaviour
    {
        [SerializeField] private List<RewardConfig> rouletteRewards;

        private static List<RewardConfig> _rewards;

        private void Awake()
        {
            _rewards = new List<RewardConfig>(rouletteRewards);
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

        public static List<RewardConfig> GetRewards()
        {
            return _rewards;
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
