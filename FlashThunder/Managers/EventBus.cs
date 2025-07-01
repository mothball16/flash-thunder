using FlashThunder.Interfaces;
using System;
using System.Collections.Generic;

namespace FlashThunder.Managers
{
    /// <summary>
    /// A simple thread-safe event bus implementation.
    /// </summary>
    public class EventBus : IEventPublisher, IEventSubscriber
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers;
        private readonly Dictionary<Type, object> _locks;

        public EventBus()
        {
            _subscribers = [];
            _locks = [];
        }

        /// <summary>
        /// Attempts to get the lock of the specified event type. If no lock is found for that type,
        /// create one.
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <returns>The lock for the event type</returns>
        private object GetLock<T>()
        {
            lock (_locks)
            {
                if (!_locks.TryGetValue(typeof(T), out var lockObj))
                {
                    lockObj = new object();
                    _locks[typeof(T)] = lockObj;
                }

                return lockObj;
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            lock (GetLock<T>())
            {
                if (_subscribers.TryGetValue(typeof(T), out var handlers))
                {
                    handlers.Add(handler);
                }
                else
                {
                    _subscribers[typeof(T)] = [handler];
                }
            }
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            lock (GetLock<T>())
            {
                if (_subscribers.TryGetValue(typeof(T), out var handlers))
                {
                    handlers.Remove(handler);
                    if (handlers.Count == 0)
                    {
                        _subscribers.Remove(typeof(T));
                    }
                }
            }
        }

        public void Publish<T>(T data)
        {
            List<Delegate> actions;
            lock (GetLock<T>())
            {
                if(!_subscribers.TryGetValue(typeof(T), out var mySubs))
                    return;
                // we need a new list to avoid concurrent modification
                actions = [.. mySubs];
            }

            foreach(var action in actions)
            {
                ((Action<T>)action)(data);
            }
        }
    }
}