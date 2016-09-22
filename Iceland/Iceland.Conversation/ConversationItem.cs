using System;
using NLua;

namespace Iceland.Conversation
{
    public class ConversationItem
    {
        public int Id { get; set; }
        public string[] Player { get; set; }
        public string[] Character { get; set; }
        public string Responses { get; set; }
        public int[] ResponseIds {
            get {
                if (Responses == null) {
                    return null;
                }

                var r = (LuaTable)LuaEngine.ExecuteScript (Responses)[0];
                int [] results = new int [r.Values.Count];
                int i = 0;
                foreach (var id in r.Values) {
                    results [i++] = Convert.ToInt32 (id);
                }

                return results;
            }
        }

        public string[] ActionScripts { get; set; }
    }
}

