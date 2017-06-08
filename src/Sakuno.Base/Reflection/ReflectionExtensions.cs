using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sakuno.Reflection
{
    public static class ReflectionExtensions
    {
        public static object FastInvoke(this ConstructorInfo construction, params object[] args) =>
            ReflectionCache.GetConstructorInvoker(construction).Invoke(args);
        public static object FastInvoke(this MethodInfo method, object instance, params object[] args) =>
            ReflectionCache.GetMethodInvoker(method).Invoke(instance, args);

        public static object FastGetValue(this FieldInfo field, object instance) =>
            ReflectionCache.GetFieldAccessor(field).GetValue(instance);
        public static void FastGetValue(this FieldInfo field, object instance, object value) =>
            ReflectionCache.GetFieldAccessor(field).SetValue(instance, value);

        public static object FastGetValue(this PropertyInfo property, object instance) =>
            ReflectionCache.GetPropertyAccessor(property).GetValue(instance);
        public static void FastSetValue(this PropertyInfo property, object instance, object value) =>
            ReflectionCache.GetPropertyAccessor(property).SetValue(instance, value);

        public static void FastAddHandler(this EventInfo @event, object instance, Delegate handler) =>
            ReflectionCache.GetEventAccessor(@event).AddHandler(instance, handler);
        public static void FastRemoveHandler(this EventInfo @event, object instance, Delegate handler) =>
            ReflectionCache.GetEventAccessor(@event).RemoveHandler(instance, handler);

        public static IEnumerable<T> FastGetCustomAttributes<T>(this Type type) where T : Attribute
        {
            return (IEnumerable<T>)ReflectionCache.CustomAttributes.GetOrAdd(type, r => new Lazy<IEnumerable<Attribute>>(() =>
            {
                if (!r.IsDefined(typeof(T)))
                    return Array.Empty<T>();

                return CustomAttributeData.GetCustomAttributes(r).Where(data => data.Constructor.DeclaringType == typeof(T)).Select(data =>
                {
                    var result = data.Constructor.FastInvoke(data.ConstructorArguments.Select(argument => argument.Value).ToArray());

                    foreach (var argument in data.NamedArguments)
                    {
                        var property = (PropertyInfo)argument.MemberInfo;

                        property.FastSetValue(result, argument.TypedValue.Value);
                    }

                    return (T)result;
                }).ToArray();
            })).Value;
        }
        public static T FastGetCustomAttribute<T>(this Type type) where T : Attribute
        {
            var attributes = (T[])type.FastGetCustomAttributes<T>();
            if (attributes.Length == 0)
                return null;

            return attributes[0];
        }
    }
}
