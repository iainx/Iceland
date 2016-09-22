using System;
using System.Collections.Generic;

using Iceland.Conversation;

namespace Iceland.Characters
{
    public class EntityModel
    {
        public string Id { get; set; }
        string name;
        public string Name { 
            get {
                if (name == null) {
                    return null;
                }

                return LuaEngine.ExecuteScript (name) [0] as string;
            }
            set {
                name = value;
            }
        }

        public string TextureName { get; set; }
        public Map.Map.Position StartPosition { get; set; }
        public Map.Map.Position ActionPosition { get; set; }

        public string [] ConversationFiles { get; set; }

        Dictionary<int, ConversationItem []> idToConversation = new Dictionary<int, ConversationItem[]> ();

        public string ConversationScript { get; set; }

        public Tuple<ConversationItem [], int> CurrentConversation {
            get {
                if (ConversationScript == null) {
                    return null;
                }

                var result = LuaEngine.ExecuteScript (ConversationScript);
                var fileIndex = Convert.ToInt32 (result [0]);
                int id = 0;
                if (result.Length > 1) {
                    id = Convert.ToInt32 (result [1]) - 1;
                }

                ConversationItem[] conv;
                if (!idToConversation.TryGetValue (fileIndex, out conv)) {
                    try {
                        conv = ConversationLoader.LoadConversationFromFile ("Conversations/" + ConversationFiles [fileIndex]);
                    } catch (Exception e) {
                        Console.WriteLine ($"{e}");
                    }
                    idToConversation [fileIndex] = conv;
                }

                return new Tuple<ConversationItem[], int> (conv, id);
            }
        }

        public string TalkCommand { get; set; }
        public string LookCommand { get; set; }
        public Dictionary<string, string> GiveCommand { get; set; }

        public string GiveFunctionForItem (string item)
        {
            string function;

            if (!GiveCommand.TryGetValue (item, out function)) {
                GiveCommand.TryGetValue ("default", out function);
            }

            return function;
        }

        public string CollectCommand { get; set; }

        public EntityModel ()
        {
        }
    }
}

