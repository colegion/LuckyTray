using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Wallet
    {
        private static Dictionary<Utility.RewardType, int> _playerRewards;
        public static bool UIShouldUpdate = false;
        public static void AddReward(Utility.RewardType type)
        {
            if (!_playerRewards.TryAdd(type, 1))
            {
                _playerRewards[type]++;
            }

            UIShouldUpdate = true;
        }

        public static Dictionary<Utility.RewardType, int> GetPlayerRewards()
        {
            return _playerRewards;
        }
    }
}
