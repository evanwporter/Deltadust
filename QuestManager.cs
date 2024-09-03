using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Deltadust {
    public class QuestManager
    {
        private List<Quest> _quests;
        private readonly RewardManager _rewardManager;

        public QuestManager()
        {
            _quests = new List<Quest>();
            _rewardManager = new RewardManager(); // Initialize the RewardManager
        }

        public void AddQuest(Quest quest)
        {
            quest.OnStarted += QuestStarted;
            quest.OnProgress += QuestProgressed;
            quest.OnObjectivesComplete += QuestObjectivesComplete;
            quest.OnFinished += QuestCompleted;

            _quests.Add(quest);
        }

        public void LoadQuestsFromFile(string filePath) {
            if (!File.Exists(filePath))
            {
                System.Diagnostics.Debug.WriteLine($"Quest file not found: {filePath}");
                return;
            }

            var json = File.ReadAllText(filePath);
            var quests = JsonConvert.DeserializeObject<List<Quest>>(json);

            foreach (var quest in quests)
            {
                AddQuest(quest);
            }
        }

        public void StartQuest(int questId) {
            var quest = _quests.FirstOrDefault(q => q.Id == questId);
            if (quest != null)
            {
                quest.Start();
            }
        }

        public void UpdateQuestProgress(ObjectiveType objectiveType, int value) {
            foreach (var quest in _quests)
            {
                quest.UpdateProgress(objectiveType, value);
            }
        }

        private void QuestStarted(object sender, EventArgs e)
        {
            Quest quest = (Quest)sender;
            System.Diagnostics.Debug.WriteLine($"Quest Started: {quest.Name}");
            // TODO
        }

        private void QuestProgressed(object sender, EventArgs e)
        {
            Quest quest = (Quest)sender;
            System.Diagnostics.Debug.WriteLine($"Quest Progressed: {quest.Name}");
            // Update game state, UI, etc.
        }

        private void QuestObjectivesComplete(object sender, EventArgs e)
        {
            Quest quest = (Quest)sender;
            System.Diagnostics.Debug.WriteLine($"Quest Objectives Complete: {quest.Name}");
            // TODO: When all objectives are complete call QuestCompleted
        }

        private void QuestCompleted(object sender, EventArgs e)
        {
            Quest quest = (Quest)sender;
            System.Diagnostics.Debug.WriteLine($"Quest Completed: {quest.Name}");

            foreach (var reward in quest.Rewards)
            {
                _rewardManager.HandleReward(reward);
            }
        }
    }
}