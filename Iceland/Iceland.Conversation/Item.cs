using System;
using System.Collections.Generic;

namespace Iceland.Conversation
{
    public class Item
    {
        public class Response {
            public string Text { get; set; }
            public int NextItem { get; set; }

            public Response ()
            {
                NextItem = -1;
            }
        }

        public string Text { get; set; }
        public List<Response> Responses { get; private set; }

        public Item ()
        {
            Responses = new List<Response> ();
        }
    }
}

