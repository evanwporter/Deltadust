using System.Collections.Generic;

namespace Deltadust.DialogueContainer
{
    public class DialogueNode
    {
        public string Text { get; set; }
        public List<DialogueOption> Options { get; set; }

        public DialogueNode()
        {
            Options = [];
        }
    }

    public class DialogueOption
    {
        public string OptionText { get; set; }
        public int NextNodeId { get; set; } // Points to the next dialogue node
    }

    public class DialogueTree
    {
        public Dictionary<int, DialogueNode> Nodes { get; set; }

        public DialogueTree()
        {
            Nodes = [];
        }
    }
}