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
            var rParametersParameter = Expression.Parameter(typeof(object[]), "rpParameters");

            var rParameterExpressions = constructor.GetParameters().Select((r, i) =>
            {
                var rParameterValue = Expression.ArrayIndex(rParametersParameter, Expression.Constant(i));

                return Expression.Convert(rParameterValue, r.ParameterType);
            }).ToArray();

            var rResult = Expression.New(constructor, rParameterExpressions);
            var rCastResult = Expression.Convert(rResult, typeof(object));

            return Expression.Lambda<Func<object[], object>>(rCastResult, rParametersParameter).Compile();
        }
    }
}
