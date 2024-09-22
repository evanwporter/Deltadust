using System;
using System.Collections.Generic;

namespace Deltadust.Quests {
    public class RewardManager
    {
        private readonly Dictionary<RewardType, Action<Reward>> _rewardActions;

        public RewardManager()
        {
            _rewardActions = new Dictionary<RewardType, Action<Reward>>
            {
                { RewardType.Experience, HandleExperienceReward },
                { RewardType.Item, HandleItemReward },
                { RewardType.TriggerActivated, HandleTriggerActivatedReward },
                { RewardType.TriggerDeactivated, HandleTriggerDeactivatedReward },
                { RewardType.FactionScore, HandleFactionScoreReward },
                { RewardType.Gold, HandleGoldReward }
            };
        }

        public void HandleReward(Reward reward)
        {
            if (_rewardActions.TryGetValue(reward.Type, out var action))
            {
                action(reward);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"No handler defined for reward type: {reward.Type}");
            }
        }

        private void HandleExperienceReward(Reward reward)
        {
            // TODO
            System.Diagnostics.Debug.WriteLine($"Player received {reward.RewardInt} experience points.");
        }

        private void HandleItemReward(Reward reward)
        {
            // TODO
            System.Diagnostics.Debug.WriteLine($"Player received item with ID {reward.RewardInt}.");
        }

        private void HandleTriggerActivatedReward(Reward reward)
        {
            // TODO
            System.Diagnostics.Debug.WriteLine("Trigger activated.");
        }

        private void HandleTriggerDeactivatedReward(Reward reward)
        {
            // TODO
            System.Diagnostics.Debug.WriteLine("Trigger deactivated.");
        }

        private void HandleFactionScoreReward(Reward reward)
        {
            // TODO
            System.Diagnostics.Debug.WriteLine($"Faction score changed by {reward.RewardInt}.");
        }

        private void HandleGoldReward(Reward reward)
        {
            // TODO
            System.Diagnostics.Debug.WriteLine($"Player received {reward.RewardInt} gold.");
        }
    }
}