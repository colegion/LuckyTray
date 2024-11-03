using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Reward Config")]
    public class RewardConfig : ScriptableObject
    {
        public Sprite rewardSprite;
        public Utility.RewardType rewardType;
    }
}
