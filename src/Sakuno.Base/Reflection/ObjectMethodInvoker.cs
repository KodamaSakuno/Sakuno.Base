using System;

namespace Sakuno.Reflection
{
    public sealed class ObjectMethodInvoker
    {
        Type _instanceType;
        string _methodName;

        MethodInvoker _invoker;

        public void Invoke(object instance, string methodName)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (methodName.IsNullOrEmpty())
                throw new ArgumentException(nameof(methodName));

            var instanceType = instance.GetType();

            if (_instanceType == instanceType && _methodName == methodName)
            {
                _invoker.Invoke(instance);
                return;
            }

            _instanceType = instanceType;
            _methodName = methodName;

            var method = _instanceType.GetMethod(_methodName);

            _invoker = ReflectionCache.GetMethodInvoker(method);
            _invoker.Invoke(instance);
        }
    }
}
