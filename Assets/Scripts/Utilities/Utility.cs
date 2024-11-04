using System;
using UnityEngine;

namespace Utilities
{
    public class Utility : MonoBehaviour
    {

        public static RewardType GetRewardTypeAsEnum(int outcome)
        {
            var type = (RewardType)outcome;
            return type;
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
    }
}
