using System;
using System.Reflection;

namespace Sakuno.Reflection
{
    public sealed class EventAccessor
    {
        public EventInfo Event { get; }

        MethodInvoker _adder, _remover;

        public EventAccessor(EventInfo @event)
        {
            Event = @event ?? throw new ArgumentNullException(nameof(@event));

            _adder = ReflectionCache.GetMethodInvoker(@event.AddMethod);
            _remover = ReflectionCache.GetMethodInvoker(@event.RemoveMethod);
        }

        public void AddHandler(object instance, Delegate handler) => _adder.Invoke(instance, handler);
        public void RemoveHandler(object instance, Delegate handler) => _remover.Invoke(instance, handler);
    }
}
