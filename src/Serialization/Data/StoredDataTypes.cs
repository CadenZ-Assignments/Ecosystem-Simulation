using System.Reflection.Emit;
using System.Text;
using Simulation_CSharp.Serialization.Data.Types;

namespace Simulation_CSharp.Serialization.Data;

public static class StoredDataTypes
{
    public static readonly Dictionary<string, KeyValuePair<Type, Func<object>>> Types;

    static StoredDataTypes()
    {
        Types = new Dictionary<string, KeyValuePair<Type, Func<object>>>();
        
        var types = typeof(TypeStoredData)
            .Assembly.GetTypes()
            .Where(type =>
                type.IsSubclassOf(typeof(TypeStoredData)) || type.GetInterfaces().Contains(typeof(ISerializable)));

        foreach (var type in types)
        {
            var name = StoredData.GetNameOfType(type);

            if (name == "TypeStoredData")
            {
                continue;
            }

            var constructor = GetConstructor(type);

            Types.Add(
                StoredData.GetNameOfType(type)!,
                new KeyValuePair<Type, Func<object>>(
                    type,
                    constructor
                )
            );
        }
    }

    private static Func<object> GetConstructor(Type type)
    {
        var t = type;

        if (type.IsGenericType)
        {
            t = type.MakeGenericType(typeof(object));
        }

        var constructorInfo = t.GetConstructor(Type.EmptyTypes);
            
        var stringBuilder = new StringBuilder(t.Name);
        var methodName = stringBuilder.ToString();
        stringBuilder.Append("Ctor");

        var dynamicMethod = new DynamicMethod(methodName, t, Type.EmptyTypes, typeof(Activator));
        var ilGenerator = dynamicMethod.GetILGenerator();
        ilGenerator.Emit(OpCodes.Newobj, constructorInfo!);
        ilGenerator.Emit(OpCodes.Ret);

        return (Func<object>) dynamicMethod.CreateDelegate(typeof(Func<object>));
    }
}