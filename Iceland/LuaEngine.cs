using System;

using NLua;

namespace Iceland
{
    public static class LuaEngine
    {
        static Lua context = new Lua ();

        static LuaEngine ()
        {
            context.LoadCLRPackage ();
        }

        public static void SetGlobalVariable (string varName, object value)
        {
            context [varName] = value;
        }

        public static T GetGlobalVariable<T> (string varName)
        {
            return (T)context [varName];
        }

        public static object[] ExecuteScript (string script, object instance)
        {
            context ["instance"] = instance;
            var res = context.DoString (script);
            context ["instance"] = null;

            return res;
        }

        public static object[] ExecuteScript (string script)
        {
            return context.DoString (script);
        }

        public static void ExecuteScriptFromFile (string script)
        {
            context.DoFile ("SoulCollectionAgencyInc/" + script);
        }
    }
}

