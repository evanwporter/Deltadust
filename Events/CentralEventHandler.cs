using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deltadust.Entities;

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
}