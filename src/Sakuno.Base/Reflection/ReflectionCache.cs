using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Sakuno.Reflection
{
    public static class ReflectionCache
    {
        static ConcurrentDictionary<ConstructorInfo, Lazy<ConstructorInvoker>> _constructorInvokers = new ConcurrentDictionary<ConstructorInfo, Lazy<ConstructorInvoker>>();
        static ConcurrentDictionary<MethodInfo, Lazy<MethodInvoker>> _methodInvokers = new ConcurrentDictionary<MethodInfo, Lazy<MethodInvoker>>();

        static ConcurrentDictionary<FieldInfo, Lazy<FieldAccessor>> _fieldAccessors = new ConcurrentDictionary<FieldInfo, Lazy<FieldAccessor>>();
        static ConcurrentDictionary<PropertyInfo, Lazy<PropertyAccessor>> _propertyAccessors = new ConcurrentDictionary<PropertyInfo, Lazy<PropertyAccessor>>();
        static ConcurrentDictionary<EventInfo, Lazy<EventAccessor>> _eventAccessors = new ConcurrentDictionary<EventInfo, Lazy<EventAccessor>>();

        internal static ConcurrentDictionary<Type, Lazy<IEnumerable<Attribute>>> CustomAttributes { get; } = new ConcurrentDictionary<Type, Lazy<IEnumerable<Attribute>>>();

        public static ConstructorInvoker GetConstructorInvoker(ConstructorInfo constructor) =>
            _constructorInvokers.GetOrAdd(constructor, r => new Lazy<ConstructorInvoker>(() => new ConstructorInvoker(r))).Value;
        public static MethodInvoker GetMethodInvoker(MethodInfo method) =>
            _methodInvokers.GetOrAdd(method, r => new Lazy<MethodInvoker>(() => new MethodInvoker(r))).Value;

        public static FieldAccessor GetFieldAccessor(FieldInfo field) =>
            _fieldAccessors.GetOrAdd(field, r => new Lazy<FieldAccessor>(() => new FieldAccessor(r))).Value;
        public static PropertyAccessor GetPropertyAccessor(PropertyInfo property) =>
            _propertyAccessors.GetOrAdd(property, r => new Lazy<PropertyAccessor>(() => new PropertyAccessor(r))).Value;
        public static EventAccessor GetEventAccessor(EventInfo @event) =>
            _eventAccessors.GetOrAdd(@event, r => new Lazy<EventAccessor>(() => new EventAccessor(r))).Value;
    }
}
