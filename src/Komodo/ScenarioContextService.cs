using System;
using TechTalk.SpecFlow;

namespace Komodo.Core
{
    public static class ScenarioContextService
    {
        public static void SaveValue<T>(T value)
        {
            if (value.Equals(default(T)))
            {
                throw new Exception("Value cannot be default value");
            }

            var key = typeof(T).FullName;
            SaveValue(key, value);
        }

        public static void SaveValue<T>(string key, T value)
        {
            if (ScenarioContext.Current.ContainsKey(key))
            {
                ScenarioContext.Current[key] = value;
            }
            else
            {
                ScenarioContext.Current.Add(key, value);
            }
        }

        public static T GetValue<T>()
        {
            var key = typeof(T).FullName;

            return GetValue<T>(key);
        }

        public static T GetValue<T>(string key)
        {
            if (!ScenarioContext.Current.ContainsKey(key))
            {
                return default(T);
            }

            return ScenarioContext.Current.Get<T>(key);
        }

        public static void ClearValue<T>()
        {
            var key = typeof(T).FullName;
            ClearValue(key);
        }

        public static void ClearValue(string key)
        {
            if (ScenarioContext.Current.ContainsKey(key))
            {
                ScenarioContext.Current.Remove(key);
            }
        }
    }
}