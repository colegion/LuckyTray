using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Wallet
    {
        private static Dictionary<Utility.RewardType, int> _playerRewards;
        public static bool UIShouldUpdate = true;
        public static void AddReward(Utility.RewardType type)
        {
            if (!_playerRewards.TryAdd(type, 1))
            {
                _playerRewards[type]++;
            }
            
            PlayerPrefs.SetInt(type.ToString(), _playerRewards[type]);

            UIShouldUpdate = true;
        }

        public static void FetchUserRewards()
        {
            _playerRewards = new Dictionary<Utility.RewardType, int>();
            for (int i = 0; i < Enum.GetValues(typeof(Utility.RewardType)).Length; i++)
            {
                var rewardCount = PlayerPrefs.GetInt(((Utility.RewardType)i).ToString(), 0);
                if (rewardCount > 0)
                {
                    _playerRewards.Add((Utility.RewardType)i, rewardCount);
                }
            }
        }

        public static Dictionary<Utility.RewardType, int> GetPlayerRewards()
        {
            return _playerRewards;
        }
    }
}
