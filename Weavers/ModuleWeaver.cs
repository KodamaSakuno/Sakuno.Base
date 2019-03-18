using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Weavers
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield break;
        }

        public override void Execute()
        {
            var targetFrameworkAttribute = ModuleDefinition.Assembly.CustomAttributes.Single(r => r.AttributeType.FullName == "System.Runtime.Versioning.TargetFrameworkAttribute");
            var frameworkName = (string)targetFrameworkAttribute.ConstructorArguments[0].Value;
            if (frameworkName.StartsWith(".NETFramework,Version=v4."))
                ProcessEnumExtensions();
        }

        void ProcessEnumExtensions()
        {
            var type = ModuleDefinition.GetType("Sakuno.EnumExtensions");

            ProcessEnumHasFlagMethod(type.GetMethod("Has"));
        }

        void ProcessEnumHasFlagMethod(MethodDefinition method)
        {
            LogInfo("Modifying Sakuno.EnumExtensions.Has<T>()...");

            var body = method.Body;
            var processor = body.GetILProcessor();

            body.Instructions.Clear();

            var genericParameterType = method.GenericParameters[0];
            var variable = new VariableDefinition(genericParameterType);

            body.Variables.Add(variable);

            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.And);
            processor.Emit(OpCodes.Ldloca_S, variable);
            processor.Emit(OpCodes.Initobj, genericParameterType);
            processor.Emit(OpCodes.Ldloc_0);
            processor.Emit(OpCodes.Cgt_Un);
            processor.Emit(OpCodes.Ret);
        }
    }
}
