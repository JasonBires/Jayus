using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jayus.Core
{
    public interface IEventManager 
    {
        void Subscribe<T>(Action<T> listener);
        void RaiseEvent<T>(T eventArgs);
    }

	public class EventManager : IEventManager
	{
        private Dictionary<Type, Delegate> eventList;

        public EventManager()
        {
            eventList = new Dictionary<Type, Delegate>();
        }

        public void Subscribe<T>(Action<T> listener)
        {
            var argType = typeof(T);
            if (eventList.Keys.Contains(argType))
            {
                var evnt = eventList[argType] as Action<T>;
                evnt += listener;
                return;
            }
            eventList.Add(typeof(T), listener);
        }

        public void RaiseEvent<T>(T eventArgs)
        {
            var argType = typeof(T);
            if (eventList.Keys.Contains(argType))
            {
                var handler = eventList[argType];
                handler.DynamicInvoke(eventArgs);
            }
        }
	}
}
