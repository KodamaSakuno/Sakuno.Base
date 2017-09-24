using Mono.Cecil;

namespace Weavers
{
    static class Extensions
    {
        public static MethodDefinition GetMethod(this TypeDefinition type, string methodName)
        {
            if (type.HasMethods)
                foreach (var method in type.Methods)
                    if (method.Name == methodName)
                        return method;

            return null;
        }
    }
}
