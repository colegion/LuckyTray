using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using UnityEngine;

namespace Utilities
{
    public static class CloudGateway
    {
        private const string DECIDE_OUTCOME_ENDPOINT = "DecideOutcome";
        private const string GET_CLAIMED_REWARDS_ENDPOINT = "GetClaimedRewards";
        private const string CHECK_ROUND_ENDPOINT = "IsRoundFinished";

        public static async Task<int> GetOutcome()
        {
            var args = new Dictionary<string, object>();
            try
            {
                var json = await CloudCodeService.Instance.CallEndpointAsync<Utility.Outcome>(DECIDE_OUTCOME_ENDPOINT, args);
                return json.outcome;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error calling Cloud Code: {e}");
                return -1;
            }
        }

        public static async Task<int[]> GetCurrentRoundClaimedRewards()
        {
            var args = new Dictionary<string, object>();
            try
            {
                var json = await CloudCodeService.Instance.CallEndpointAsync<Utility.ClaimedRewards>(GET_CLAIMED_REWARDS_ENDPOINT, args);
                return json.rewards;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error calling Cloud Code: {e}");
                return new int[]{};
            }
        }

        public static async Task<bool> IsRoundFinished()
        {
            var args = new Dictionary<string, object>();
            try
            {
                var json = await CloudCodeService.Instance.CallEndpointAsync<Utility.RoundStatus>(CHECK_ROUND_ENDPOINT, args);
                return json.isFinished;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error calling Cloud Code: {e}");
                return false;
            }
        }
    }
}
