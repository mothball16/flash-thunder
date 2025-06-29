using System;
using System.Collections.Generic;

namespace FlashThunder.Managers
{
    public static class EventBus
    {

        private static class Events<T>
        {
            public static readonly List<Action<T>> Subscribers = [];
        }

        private static readonly Dictionary<Type, object> _locks = [];

        private static object GetLock<T>()
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

        public static void Subscribe<T>(Action<T> handler)
        {
            lock (GetLock<T>())
            {
                Events<T>.Subscribers.Add(handler);
            }
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            lock (GetLock<T>())
            {
                Events<T>.Subscribers.Remove(handler);
            }
        }

        public static void Publish<T>(T data)
        {
            List<Action<T>> subscribers;
            lock (GetLock<T>())
            {
                subscribers = [.. Events<T>.Subscribers];
            }
            foreach(var action in subscribers)
            {
                action(data);
            }
        }
    }
}