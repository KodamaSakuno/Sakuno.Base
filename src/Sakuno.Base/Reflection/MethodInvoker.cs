using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class MethodInvoker
    {
        public MethodInfo Method { get; }

        Func<object, object[], object> _invoker;

        public MethodInvoker(MethodInfo method)
        {
            Method = method ?? throw new ArgumentNullException(nameof(method));

            _invoker = CreateInvokerCore(method);
        }

        public object Invoke(object instance, params object[] args) =>
            _invoker(instance, args);

        static Func<object, object[], object> CreateInvokerCore(MethodInfo method)
        {
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var argsParameter = Expression.Parameter(typeof(object[]), "parameters");

            var arguments = method.GetParameters().Select((r, i) =>
            {
                var parameterValue = Expression.ArrayIndex(argsParameter, Expression.Constant(i));

                return Expression.Convert(parameterValue, r.ParameterType);
            }).ToArray();

            var castInstance = !method.IsStatic ? Expression.Convert(instanceParameter, method.ReflectedType) : null;
            var call = Expression.Call(castInstance, method, arguments);

            if (call.Type == typeof(void))
            {
                var func = Expression.Lambda<Action<object, object[]>>(call, instanceParameter, argsParameter).Compile();

                return (instance, args) =>
                {
                    func(instance, args);

                    return null;
                };
            }
            else
            {
                var castReturnValue = Expression.Convert(call, typeof(object));

                return Expression.Lambda<Func<object, object[], object>>(castReturnValue, instanceParameter, argsParameter).Compile();
            }
        }
    }
}
