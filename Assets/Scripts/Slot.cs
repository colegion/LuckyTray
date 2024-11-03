using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Slot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rewardField;
    [SerializeField] private SpriteRenderer slotHighlight;
    [SerializeField] private List<SlotState> states;
    [SerializeField] private SpriteRenderer tickSprite;

    public void ConfigureSelf(RewardConfig config)
    {
        rewardField.sprite = config.rewardSprite;
    }

    
    [Serializable]
    internal class SlotState
    {
        public Utility.SlotStatus slotStatus;
        public Sprite statusSprite;
    }
}
