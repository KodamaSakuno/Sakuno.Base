using System.Collections.Generic;
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
            ProcessSizeOfMethod();
            ProcessUnsafeOperations();
            ProcessEnumExtensions();
        }

        void ProcessSizeOfMethod()
        {
            LogInfo("Modifying Sakuno.TypeUtil.SizeOf<T>()...");

            var type = ModuleDefinition.GetType("Sakuno.TypeUtil");
            var method = type.GetMethod("SizeOf");
            var body = method.Body;
            var processor = body.GetILProcessor();

            body.Instructions.Clear();

            processor.Emit(OpCodes.Sizeof, method.GenericParameters[0]);
            processor.Emit(OpCodes.Ret);
        }

        void ProcessUnsafeOperations()
        {
            var type = ModuleDefinition.GetType("Sakuno.UnsafeOperations");

            ProcessZeroMemoryMethod(type.GetMethod("ZeroMemory"));
            ProcessCopyMemoryMethod(type.GetMethod("CopyMemory"));
            ProcessAsMethod(type.GetMethod("As"));
        }
        void ProcessZeroMemoryMethod(MethodDefinition method)
        {
            LogInfo("Modifying Sakuno.UnsafeOperations.ZeroMemory()...");

            var body = method.Body;
            var processor = body.GetILProcessor();

            body.Instructions.Clear();

            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldc_I4_0);
            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.Initblk);
            processor.Emit(OpCodes.Ret);
        }
        void ProcessCopyMemoryMethod(MethodDefinition method)
        {
            LogInfo("Modifying Sakuno.UnsafeOperations.CopyMemory()...");

            var body = method.Body;
            var processor = body.GetILProcessor();

            body.Instructions.Clear();

            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldarg_2);
            processor.Emit(OpCodes.Cpblk);
            processor.Emit(OpCodes.Ret);
        }
        void ProcessAsMethod(MethodDefinition method)
        {
            LogInfo("Modifying Sakuno.UnsafeOperations.As<T1, T2>()...");

            var body = method.Body;
            var processor = body.GetILProcessor();

            body.Instructions.Clear();

            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ret);
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
