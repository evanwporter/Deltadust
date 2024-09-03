using System;
using Deltadust.Events;
using Microsoft.Xna.Framework;

namespace Deltadust {
    [Serializable]
    public class Objective
    {
        public ObjectiveType Type {get; set;}
        public int ObjectiveInt {get; set;}
        public int Progress {get; set;}
        public int Specifier {get; set;} // For monster quests where you need to kill a certain type, should be ID

        public Objective(ObjectiveType type, int objective, CentralEventHandler eventHandler) {
            Type = type;
            ObjectiveInt = objective;

            if (Type == ObjectiveType.EnemiesKilled) {
                eventHandler.MonsterKilled += OnKilled;
            }
        }

        private void OnKilled(object sender, MonsterKillEventArgs e) {
            if (e.Monster.ID_Name == Specifier) {
                ObjectiveInt++;
                Console.WriteLine($"Monster killed: {e.Monster.Name}. Progress: {Progress}/{ObjectiveInt}");

                if (Progress >= ObjectiveInt) {
                    Console.WriteLine("Objective complete: All required monsters killed.");
                    // TODO
                }
            }
        }
    }

    [Serializable]
    public enum ObjectiveType {
        Unknown = 0,
        ItemsHeld = 1,
        Gold = 2,
        EnemiesKilled = 3,
        DestinationReached = 4,
        TriggerEnabled = 5,
    }
}