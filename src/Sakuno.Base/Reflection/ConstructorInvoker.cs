using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sakuno.Reflection
{
    public class ConstructorInvoker
    {
        public ConstructorInfo Constructor { get; }

        Func<object[], object> _invoker;

        public ConstructorInvoker(ConstructorInfo constructor)
        {
            Constructor = constructor ?? throw new ArgumentNullException(nameof(constructor));

            _invoker = CreateInvokerCore(constructor);
        }

        public object Invoke(params object[] args) => _invoker(args);

        static Func<object[], object> CreateInvokerCore(ConstructorInfo constructor)
        {
            var argsParameter = Expression.Parameter(typeof(object[]), "args");

            var arguments = constructor.GetParameters().Select((r, i) =>
            {
                var parameterValue = Expression.ArrayIndex(argsParameter, Expression.Constant(i));

                return Expression.Convert(parameterValue, r.ParameterType);
            }).ToArray();

            var result = Expression.New(constructor, arguments);
            var castResult = Expression.Convert(result, typeof(object));

            return Expression.Lambda<Func<object[], object>>(castResult, argsParameter).Compile();
        }
    }
}
