using System;

using Deltadust.Entities;
using Deltadust.ItemManagement;

namespace Deltadust.Events
{
    public class CentralEventHandler {
        public event EventHandler<MonsterKillEventArgs> MonsterKilled;

        public void OnKill(Slime monster) {
            // Raise the MonsterKilled event
            MonsterKilled?.Invoke(this, new MonsterKillEventArgs { Monster = monster });
        }
    }

    public class MonsterKillEventArgs : EventArgs {
        public Slime Monster { get; set; }
    }

    public class ItemFoundEventArgs : EventArgs {
        public Item Item { get; set; }
    }
}