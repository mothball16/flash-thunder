using System;

namespace FlashThunder.Managers;

public interface IEventPublisher
{
    void Publish<T>(T data);
}

public interface IEventSubscriber
{
    EventConnection<T> Subscribe<T>(Action<T> handler);
    void Unsubscribe<T>(Action<T> handler);
}