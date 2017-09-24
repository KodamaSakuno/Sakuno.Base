using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Weavers
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public void Execute()
        {
            ProcessSizeOfMethod();
            ProcessUnsafeOperations();
            ProcessEnumExtensions();
        }

        void ProcessSizeOfMethod()
        {
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
        }
        void ProcessZeroMemoryMethod(MethodDefinition method)
        {
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
            var body = method.Body;
            var processor = body.GetILProcessor();

            body.Instructions.Clear();

            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.Ldarg_2);
            processor.Emit(OpCodes.Cpblk);
            processor.Emit(OpCodes.Ret);
        }

        void ProcessEnumExtensions()
        {
            var type = ModuleDefinition.GetType("Sakuno.EnumExtensions");

            ProcessEnumHasFlagMethod(type.GetMethod("Has"));
        }

        void ProcessEnumHasFlagMethod(MethodDefinition method)
        {
            var body = method.Body;
            var processor = body.GetILProcessor();

            body.Instructions.Clear();

            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Ldarg_1);
            processor.Emit(OpCodes.And);
            processor.Emit(OpCodes.Ldc_I4_0);
            processor.Emit(OpCodes.Cgt_Un);
            processor.Emit(OpCodes.Ret);
        }
    }
}
