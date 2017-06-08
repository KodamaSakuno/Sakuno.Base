using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class PropertyAccessor
    {
        public PropertyInfo Property { get; }

        Func<object, object> _getter;
        Action<object, object> _setter;

        public PropertyAccessor(PropertyInfo property)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));

            _getter = CreateGetter(property);
            _setter = CreateSetter(property);
        }

        public object GetValue(object instance)
        {
            if (_getter == null)
                throw new NotSupportedException("Getter is not defined.");

            return _getter(instance);
        }
        public void SetValue(object instance, object value)
        {
            if (_setter == null)
                throw new NotSupportedException("Setter is not defined.");

            _setter(instance, value);
        }

        static Func<object, object> CreateGetter(PropertyInfo property)
        {
            if (!property.CanRead)
                return null;

            var instanceParameter = Expression.Parameter(typeof(object), "instance");

            var castInstance = !property.GetMethod.IsStatic ? Expression.Convert(instanceParameter, property.ReflectedType) : null;

            var result = Expression.Property(castInstance, property);
            var castResult = Expression.Convert(result, typeof(object));

            return Expression.Lambda<Func<object, object>>(castResult, instanceParameter).Compile();
        }
        static Action<object, object> CreateSetter(PropertyInfo property)
        {
            if (!property.CanWrite)
                return null;

            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var valueParameter = Expression.Parameter(typeof(object), "value");

            var castInstance = !property.GetMethod.IsStatic ? Expression.Convert(instanceParameter, property.ReflectedType) : null;
            var castValue = Expression.Convert(valueParameter, property.PropertyType);

            var propertyAccess = Expression.Property(castInstance, property);
            var propertyAssign = Expression.Assign(propertyAccess, castValue);

            return Expression.Lambda<Action<object, object>>(propertyAssign, instanceParameter, valueParameter).Compile();
        }
    }
}
