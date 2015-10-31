using System;
using System.Collections.Generic;
using Foundation;

namespace Iceland.Conversation
{
    // NSObject so we can pass it into NSDictionary
    public class Details : NSObject
    {
        public string Introduction { get; set; }
        public List<Item> Items { get; private set; }

        public Details ()
        {
            Items = new List<Item> ();
        }
    }
}

