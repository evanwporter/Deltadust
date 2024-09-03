using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;


namespace Deltadust {
    public delegate void QuestEventHandler(object sender, EventArgs e);

    [Serializable]
    public class Quest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Reward> Rewards { get; private set; }
        public IList<Requirement> Requirements { get; private set; }
        public IList<Objective> Objectives { get; private set; }

        public event QuestEventHandler OnStarted;
        public event QuestEventHandler OnProgress;
        public event QuestEventHandler OnObjectivesComplete;
        public event QuestEventHandler OnFinished;

        private bool _isCompleted;

        public Quest()
        {
            Rewards = new List<Reward>();
            Requirements = new List<Requirement>();
            Objectives = new List<Objective>();
            _isCompleted = false;
        }

        public void Start()
        {
            OnStarted?.Invoke(this, EventArgs.Empty);
            CheckProgress();
        }

        public void UpdateProgress(ObjectiveType objectiveType, int value)
        {
            var objective = Objectives.FirstOrDefault(o => o.Type == objectiveType);
            if (objective != null)
            {
                if (objective.ObjectiveInt <= value)
                {
                    objective.ObjectiveInt = value;
                    OnProgress?.Invoke(this, EventArgs.Empty);
                }
                CheckProgress();
            }
        }

        private void CheckProgress()
        {
            if (Objectives.All(o => o.ObjectiveInt > 0))  // Assumes 0 means incomplete
            {
                OnObjectivesComplete?.Invoke(this, EventArgs.Empty);
                CompleteQuest();
            }
        }

        private void CompleteQuest()
        {
            if (!_isCompleted)
            {
                _isCompleted = true;
                OnFinished?.Invoke(this, EventArgs.Empty);
                // Reward the player
            }
        }
    }

    [Serializable]
    public class Requirement {
        public RequirementType Type { get; set; }
        public float RequiredNumber { get; set; }
        public string RequiredString { get; set; }
    }

    [Serializable]
    public enum RequirementType {
        Unknown = 0,
        Level = 1,
        Class = 2,
        Race = 3,
        FactionScore = 4,
        TriggerActive = 5,
        TriggerDeactive = 6,
        ItemHeld = 7
    }

    [Serializable]
    public class Reward {
        public RewardType Type { get; set; }
        public float RewardFloat { get; set; }
        public int RewardInt { get; set; }
    }

    [Serializable]
    public enum RewardType {
        Unknown = 0,
        Experience = 1,
        Item = 2,
        TriggerActivated = 3,
        TriggerDeactivated = 4,
        FactionScore = 5,
        Gold = 6 
    }
}