using System;
using UnityEngine;

namespace Utilities
{
    public class Utility : MonoBehaviour
    {

        public static RewardType GetRewardTypeAsEnum(string outcome)
        {
            // Try to parse the string to the RewardType enum
            if (Enum.TryParse<RewardType>(outcome, true, out var rewardType))
            {
                return rewardType;
            }
    
            // If parsing fails, throw an exception
            throw new Exception("Undefined enum type!");
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
            public string outcome;
        }
    }
}
