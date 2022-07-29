using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Program
{
	public static void Main()
	{
		// https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.invoke?redirectedfrom=MSDN&view=net-6.0#overloads:~:text=a%20MethodBuilder.-,Examples,-The%20following%20code
		// Get Class
		//Type myType = Type.GetType("MyTestClass");
		Type myType = typeof(MyTestClass);
		
		// Create Class
		//ConstructorInfo classConstructor = myType.GetConstructor(Type.EmptyTypes);
		//object classObject = classConstructor.Invoke(new object[]{});
		MyTestClass classObject = new MyTestClass();
		
		// Get the Method
		string methodName = nameof(MyTestClass.MyMethod);
		MethodInfo myMethod = myType.GetMethod(methodName);
		
		// Add Parameters
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		parameters.Add("message", "Hello");
		
		// Invoke Method
		myMethod.InvokeWithNamedParameters(classObject, parameters);
		
		MyTestClass mtc = new MyTestClass();
		mtc.MyMethod();
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

// https://stackoverflow.com/a/13072614
public static class ReflectionExtensions 
{
	public static object InvokeWithNamedParameters(this MethodBase self, object obj, IDictionary<string, object> namedParameters) 
	{ 
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
