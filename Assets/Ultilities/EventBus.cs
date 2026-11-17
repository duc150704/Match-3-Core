using System;
using System.Collections.Generic;

public static class EventBus 
{
    private static Dictionary<Type, Delegate> _events;

    public static void Subscribe<T>(Action<T> e) where T : IEvent
    {
        var type = typeof(T);

        if (_events.ContainsKey(type))
            _events[type] = Delegate.Combine(_events[type], e);
        else
            _events[type] = e;
    }

    public static void Unsubscribe<T>(Action<T> e) where T : IEvent
    {
        var type = typeof(T);

        if (!_events.ContainsKey(type))
            return;

        _events[type] = Delegate.Remove(_events[type], e);
        
        if (_events[type] == null)
            _events.Remove(type);
    }

    public static void Public<T>(T eventData) where T : IEvent
    {
        var type = typeof(T);

        if (!_events.ContainsKey(type))
            return;

        (_events[type] as Action<T>)?.Invoke(eventData);
    }

    public static void Clear()
        => _events.Clear();
}
