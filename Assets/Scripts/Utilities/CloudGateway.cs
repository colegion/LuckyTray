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

        public static async Task<string> GetOutcome()
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
                return null;
            }
        }
    }
}
