using System;
using System.Collections.Generic;

namespace EventManagers
{
    public class EventManager
    {
        /// <summary>
        /// EventManager.FunctionName("KeyName" , Action)
        /// </summary>
        private static readonly Dictionary<string, Action> _eventDictionary = new Dictionary<string, Action>();

        public static void StartListening(string eventName, Action listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent += listener;
                _eventDictionary[eventName] = thisEvent;
            }
            else
            {
                _eventDictionary.Add(eventName, listener);
            }
        }

        public static void StopListening(string eventName, Action listener)
        {
            if (_eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent -= listener;
                _eventDictionary[eventName] = thisEvent;
            }
            else
            {
                _eventDictionary.Remove(eventName);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            if (_eventDictionary.TryGetValue(eventName, out Action thisEvent))
            {
                thisEvent?.Invoke();
            }
        }
    }
}