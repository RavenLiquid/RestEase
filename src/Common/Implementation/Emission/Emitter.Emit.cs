using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using RestEase.Implementation.Analysis;
using RestEase.Platform;

namespace RestEase.Implementation.Emission
{
    internal class Emitter
    {
        private readonly ModuleBuilder moduleBuilder;
        private int numTypes;

        public Emitter(ModuleBuilder moduleBuilder)
        {
            this.moduleBuilder = moduleBuilder;
        }

        public TypeEmitter EmitType(TypeModel type)
        {
            var typeBuilder = this.moduleBuilder.DefineType(this.CreateImplementationName(type.Type), TypeAttributes.Public | TypeAttributes.Sealed);

            return new TypeEmitter(typeBuilder, type);
        }

        private string CreateImplementationName(Type interfaceType)
        {
            int numTypes = Interlocked.Increment(ref this.numTypes);
            var typeInfo = interfaceType.GetTypeInfo();
            string name = typeInfo.IsGenericType ? typeInfo.GetGenericTypeDefinition().FullName! : typeInfo.FullName!;
            return "RestEase.AutoGenerated.<>" + name.Replace('.', '+') + "_" + numTypes;
        }
    }
}