using System;

using Deltadust.Entities.Animated.Monsters;
using Deltadust.ItemManagement;

namespace Deltadust.Events
{
    public class EventManager {
        public event EventHandler<DialogueEventArgs> OnDialogueTriggered;
        public event EventHandler<MonsterKillEventArgs> MonsterKilled;

        public void OnKill(Monster monster) {
            // Raise the MonsterKilled event
            MonsterKilled?.Invoke(this, new MonsterKillEventArgs { Monster = monster });
        }

        public void TriggerDialogue(string dialogueText) {
            OnDialogueTriggered?.Invoke(this, new DialogueEventArgs { DialogueText = dialogueText });
        }
    }

    public class MonsterKillEventArgs : EventArgs {
        public Monster Monster { get; set; }
    }

    public class ItemFoundEventArgs : EventArgs {
        public Item Item { get; set; }
    }

    public class DialogueEventArgs : EventArgs
    {
        public string DialogueText { get; set;}
    }
}