using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public sealed class FieldAccessor
    {
        public FieldInfo Field { get; }

        Func<object, object> _getter;
        Action<object, object> _setter;

        public FieldAccessor(FieldInfo field)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));

            _getter = CreateGetter(field);
            _setter = CreateSetter(field);
        }

        public object GetValue(object instance) => _getter(instance);
        public void SetValue(object instance, object value) => _setter(instance, value);

        static Func<object, object> CreateGetter(FieldInfo field)
        {
            var instanceParameter = Expression.Parameter(typeof(object), "instance");

            var castInstance = !field.IsStatic ? Expression.Convert(instanceParameter, field.ReflectedType) : null;

            var result = Expression.Field(castInstance, field);
            var castResult = Expression.Convert(result, typeof(object));

            return Expression.Lambda<Func<object, object>>(castResult, instanceParameter).Compile();
        }
        static Action<object, object> CreateSetter(FieldInfo field)
        {
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var valueParameter = Expression.Parameter(typeof(object), "value");

            var castInstance = !field.IsStatic ? Expression.Convert(instanceParameter, field.ReflectedType) : null;
            var castValue = Expression.Convert(valueParameter, field.FieldType);

            var fieldAccess = Expression.Field(castInstance, field);
            var fieldAssign = Expression.Assign(fieldAccess, castValue);

            return Expression.Lambda<Action<object, object>>(fieldAssign, instanceParameter, valueParameter).Compile();
        }
    }
}
