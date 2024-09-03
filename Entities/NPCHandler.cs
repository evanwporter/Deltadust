using System;
using System.Collections.Generic;
using Deltadust.Entities;

namespace Deltadust {
    public class NPCManager {
        public event EventHandler<NPCEventArgs> NPCKilled;

        private List<NPC> _NPCs;

        public NPCManager() {
            _NPCs = new List<NPC>();
        }

        public void AddNPC(NPC npc) {
            _NPCs.Add(npc);
            npc.OnKilled += HandleNPCKilled;
        }

        private void HandleNPCKilled(object sender, EventArgs e) {
            var NPC = (NPC)sender;
            _NPCs.Remove(NPC);

            // Raise the NPCKilled event
            OnNPCKilled(NPC);
        }

        protected virtual void OnNPCKilled(NPC NPC) {
            NPCKilled?.Invoke(this, new NPCEventArgs { NPCType = NPC.GetType().Name });
        }
    }

    public class NPCEventArgs : EventArgs {
        public string NPCType { get; set; }
    }
}
