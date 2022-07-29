using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Program
{
	public static void Main()
	{
		Type magicType = Type.GetType("MyTestClass");
        ConstructorInfo magicConstructor = magicType.GetConstructor(Type.EmptyTypes);
        object magicClassObject = magicConstructor.Invoke(new object[]{});

        // Get the ItsMagic method and invoke with a parameter value of 100

        MethodInfo magicMethod = magicType.GetMethod("MyMethod");
		
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		parameters.Add("message", "Hello");
		magicMethod.InvokeWithNamedParameters(magicClassObject, parameters);
	}
}

public class MyTestClass
{
	public MyTestClass()
	{
	}
	
	public void MyMethod(string message = "Test")
	{
		Console.WriteLine(message);
	}
}

public static class ReflectionExtensions {

    public static object InvokeWithNamedParameters(this MethodBase self, object obj, IDictionary<string, object> namedParameters) { 
        return self.Invoke(obj, MapParameters(self, namedParameters));
    }

    public static object[] MapParameters(MethodBase method, IDictionary<string, object> namedParameters)
    {
        string[] paramNames = method.GetParameters().Select(p => p.Name).ToArray();
        object[] parameters = new object[paramNames.Length];
        for (int i = 0; i < parameters.Length; ++i) 
        {
            parameters[i] = Type.Missing;
        }
        foreach (var item in namedParameters)
        {
            var paramName = item.Key;
            var paramIndex = Array.IndexOf(paramNames, paramName);
            if (paramIndex >= 0)
            {
                parameters[paramIndex] = item.Value;
            }
        }
        return parameters;
    }
}
