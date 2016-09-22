using System;
using System.IO;

using Newtonsoft.Json;

namespace Iceland.Conversation
{
    public static class ConversationLoader
    {
        public static ConversationItem[] LoadConversationFromFile (string filename)
        {
            string contents = File.ReadAllText (filename);
            return JsonConvert.DeserializeObject<ConversationItem[]> (contents);
        }
    }
}